import React, { useState } from 'react';
import { Col } from 'reactstrap';
import ShrtnForm from './ShrtnForm/ShrtnForm';
import ShrtndUrls from './ShrtndUrls/ShrtndUrls';

export const Home = () => {
  const [shrtndUrlsState, updateShrtndUrlsState] = useState({
    loading: false,
    urls: []
  });

  const onSubmit = async (syntheticEvent) => {
    syntheticEvent.preventDefault();
    updateShrtndUrlsState({...shrtndUrlsState, loading: true});
    
    const myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");
  
    const options = {
      method: 'POST',
      body: JSON.stringify(syntheticEvent.target[0].value),
      headers: myHeaders
    };
  
    const response = await fetch("api/shrtn", options);
  
    if (response.ok) {
      const {url, code} = await response.json()
      console.log(url, code)
      updateShrtndUrlsState({
        ...shrtndUrlsState, 
        urls: shrtndUrlsState.urls.concat([{url: url, code: code}]),
        loadingState: false,
      });
    } else {
      updateShrtndUrlsState({...shrtndUrlsState, loading: false});
    }
  }

  return (
    <>
      <h1>Shrtn your Url</h1>

      <Col>
        <ShrtnForm onSubmit={onSubmit} loading={shrtndUrlsState.loading}/>
        {shrtndUrlsState.urls.length > 0 && <ShrtndUrls urls={shrtndUrlsState.urls} />}
      </Col>
    </>
  )
}

export default Home;
