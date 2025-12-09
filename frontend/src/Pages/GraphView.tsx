import React, {lazy} from 'react';

const NoSSRForceGraph = lazy(()=> import ('../Config/NoSSRForceGraph'))

const myData = {
  nodes: [{id:"a"}, {id:"b"}, {id:"c"}],
  links: [{source: "a", target: "b"}, {source: "c", target: "a"}]
}

export default function GraphView() {
  return <NoSSRForceGraph graphData={myData}/>
}