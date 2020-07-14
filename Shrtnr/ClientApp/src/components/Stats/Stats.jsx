import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Col } from 'reactstrap';

export const Stats = () => {
  const { code } = useParams()
  const [statsState, updateStatsState] = useState({
    loading: false,
    stats: undefined,
  })

  useEffect(() => {
    async function fetchData() {
      updateStatsState(state => ({...state, loading: true}))
      const response = await fetch(`/api/stats/${code}`);
      if (response.ok) {
        const data = await response.json();
        console.log(data);
        updateStatsState(state => ({...state, loading: false, stats: data}));
      } else {
        updateStatsState(state => ({...state, loading: false}));
      }
    }
    fetchData();
  }, [code]);

  if (statsState.loading) return "Loading..."

  return (
    <>
      <h1>Stats</h1>

      <Col>
        { statsState.stats ? 
          <StatsItem 
            url={statsState.stats.url}
            code={statsState.stats.code}
            clicks={statsState.stats.clicks} 
          /> 
        : 
          "Sorry couldn't load your stats." 
        }
      </Col>
    </>
  )
}

export const StatsItem = (props) => (
  <div className="stats-item">
    <div>URL: {props.url}</div>
    <div>Shrt URL: {`${window.location.origin}/${props.code}`}</div>
    <div>Clicks: {props.clicks}</div>
  </div>
)

export default Stats;