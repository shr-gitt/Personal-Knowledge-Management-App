import React, {lazy} from 'react';
import { useEffect, useState } from 'react';
import { fetchUserGraph } from '../Service/graphService';

const NoSSRForceGraph = lazy(()=> import ('../Config/NoSSRForceGraph'))

export default function GraphView() {
  const [data, setData] = useState({ nodes: [], links: [] });

  useEffect (()=>{
    const fetchData = async () => {
      try{
        let Data = await fetchUserGraph();
        setData(Data);
      }
      catch(err){
        console.error(err);
      }
    };
    fetchData();
  },[])

  return <NoSSRForceGraph graphData={data}/>
}