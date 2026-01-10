import React, { useRef, useEffect, useState } from "react";
import * as d3 from "d3";
import { fetchUserGraph } from "../Service/graphService";

export default function GraphView() {
  const svgRef = useRef<SVGSVGElement | null>(null);
  const [data, setData] = useState<{ nodes: any[]; links: any[] }>({
    nodes: [],
    links: [],
  });

  type GraphNode = {
    id: string;
    group?: number;
    x?: number;
    y?: number;
    fx?: number | null;
    fy?: number | null;
    label?: string;
  };

  type GraphLink = {
    source: string | GraphNode;
    target: string | GraphNode;
    value?: number;
  };

  useEffect(() => {
    fetchUserGraph().then(setData).catch(console.error);
  }, []);

  useEffect(() => {
    if (!data.nodes.length) return;

    const width = 928;
    const height = 680;
    const color = d3.scaleOrdinal(d3.schemeCategory10);

    // Split the label into multiple lines based on space (or any custom separator)
    const splitLabel = (label: string, maxLength: number) => {
      const words = label.split(" ");
      let lines: string[] = [];
      let currentLine = "";

      words.forEach((word) => {
        if ((currentLine + word).length > maxLength) {
          lines.push(currentLine);
          currentLine = word;
        } else {
          currentLine += currentLine ? " " + word : word;
        }
      });

      if (currentLine) lines.push(currentLine);
      return lines;
    };

    const nodes = data.nodes.map((node) => ({
      ...node,
      label: node.label || node.id, // Default to ID if label is not provided
    }));

    const svg = d3
      .select(svgRef.current)
      .attr("width", width)
      .attr("height", height)
      .attr("viewBox", [-width / 2, -height / 2, width, height]);

    svg.selectAll("*").remove(); // cleanup previous render

    const links = data.links.map((d) => ({ ...d }));

    const simulation = d3
      .forceSimulation<GraphNode>(nodes)
      .force(
        "link",
        d3
          .forceLink<GraphNode, GraphLink>(links)
          .id((d) => d.id)
          .distance(100)
      ) // Increase distance between linked nodes
      .force("charge", d3.forceManyBody().strength(-300)) // Increase repulsive force (stronger charge)
      .force("center", d3.forceCenter(0, 0)); // Keep nodes centered

    const link = svg
      .append("g")
      .attr("stroke", "#999")
      .attr("stroke-opacity", 0.6)
      .selectAll("line")
      .data(links)
      .join("line");

    const node = svg
      .append("g")
      .attr("stroke", "#fff")
      .attr("stroke-width", 1.5)
      .selectAll("g")
      .data(nodes)
      .join("g")
      .call(
        d3
          .drag<SVGGElement, GraphNode>()
          .on("start", dragstarted)
          .on("drag", dragged)
          .on("end", dragended)
      );

    // Circle for each node
    node
      .append("circle")
      .attr("r", (d) => {
        // Create an invisible text element to measure the label size
        const tempText = svg
          .append("text")
          .attr("font-size", 12)
          .attr("visibility", "hidden")
          .text(d.label);

        // Measure the width and height of the text
        const bbox = tempText.node()?.getBBox();
        tempText.remove(); // Remove the temporary text element

        // Use the bounding box to determine the radius
        const padding = 6; // Padding around the label
        const radius = Math.max(
          12,
          Math.max(bbox?.width || 0, bbox?.height || 0) / 2 + padding
        );
        return radius;
      })
      .attr("fill", (d) => color(d.group))
      .attr("stroke", "#050000ff")
      .attr("stroke-width", 1.5);

    // Label for each node: Split into multiple lines
    const label = node
      .append("g")
      .selectAll("text")
      .data((d) => splitLabel(d.label, 20)) // Split into lines with max length of 20 characters
      .join("text")
      .text((d) => d)
      .attr("text-anchor", "middle")
      .attr("font-size", 12)
      .attr("fill", "#ae1f1fff")
      .attr("pointer-events", "none")
      .attr("dy", (d, i) => `${i * 1.2}em`) // Add vertical spacing for multiple lines
      .attr("y", (d) => -5);

    simulation.on("tick", () => {
      link
        .attr("x1", (d) => (d.source as GraphNode).x ?? 0)
        .attr("y1", (d) => (d.source as GraphNode).y ?? 0)
        .attr("x2", (d) => (d.target as GraphNode).x ?? 0)
        .attr("y2", (d) => (d.target as GraphNode).y ?? 0);

      // Update both circle and label position via transform
      node.attr("transform", (d) => `translate(${d.x ?? 0}, ${d.y ?? 0})`);
    });

    function dragstarted(event: d3.D3DragEvent<SVGGElement, GraphNode, any>) {
      if (!event.active) simulation.alphaTarget(0.3).restart();
      event.subject.fx = event.subject.x;
      event.subject.fy = event.subject.y;
    }

    function dragged(event: d3.D3DragEvent<SVGGElement, GraphNode, any>) {
      event.subject.fx = event.x;
      event.subject.fy = event.y;
    }

    function dragended(event: d3.D3DragEvent<SVGGElement, GraphNode, any>) {
      if (!event.active) simulation.alphaTarget(0);
      event.subject.fx = null;
      event.subject.fy = null;
    }

    return () => simulation.stop();
  }, [data]);

  return <svg ref={svgRef} style={{ maxWidth: "100%", height: "auto" }} />;
}
