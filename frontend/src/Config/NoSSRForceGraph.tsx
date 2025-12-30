import { useEffect, useRef } from "react";
import ForceGraph2D from "react-force-graph-2d";

export default function NoSSRForceGraph({ graphData }) {
  const fgRef = useRef<any>(null);

  useEffect(() => {
    if (fgRef.current && graphData.nodes.length) {
      fgRef.current.zoomToFit(400);
    }
  }, [graphData]);

  return (
    <ForceGraph2D
      graphData={graphData}
      nodeAutoColorBy="type"
      nodeLabel={(node) => node.label}
      linkColor={() => "#444"}
      nodeCanvasObject={(node, ctx, globalScale) => {
        if (node.x === undefined || node.y === undefined) return;

        const label = node.label;
        const fontSize = 12 / globalScale;

        // Measure text width
        const textWidth = ctx.measureText(label).width;
        const radius = Math.max(5, textWidth / 2 + 6); // circle radius dynamic + padding

        // Draw node circle
        ctx.beginPath();
        ctx.arc(node.x, node.y, radius, 0, 2 * Math.PI);
        ctx.fillStyle = node.color || "#4f46e5";
        ctx.fill();

        // Draw text INSIDE node
        ctx.font = `${fontSize}px Inter, sans-serif`;
        ctx.textAlign = "center";
        ctx.textBaseline = "middle";
        ctx.fillStyle = "#ffffff";
        ctx.fillText(label, node.x, node.y);
      }}
      nodeRelSize={6}
    />
  );
}
