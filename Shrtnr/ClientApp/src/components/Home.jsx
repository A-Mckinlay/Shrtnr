import React from 'react';
import { Form, Input, Button, Col, Row } from 'reactstrap';

const onSubmit = async (syntheticEvent) => {
  syntheticEvent.preventDefault();
  
  const myHeaders = new Headers();
  myHeaders.append("Content-Type", "application/json");

  const options = {
    method: 'POST',
    body: JSON.stringify(syntheticEvent.target[0].value),
    headers: myHeaders
  };

  const response = await fetch("/shrtn", options);

  if (response.ok) {
    const data = await response.json()
    console.log(data);
  }
}

export const Home = (props) => (
  <>
    <h1>Shrtn your Url</h1>

    <Col>
      <Form onSubmit={onSubmit}>
        <Row form>
          <Col md={11}>
            <Input 
              type="url"
              name="url"
              placeholder="www.example.com/looooooooooooong"
              bsSize="lg"
            />
          </Col>
          <Col md={1}>
            <Button size="lg">&gt;</Button>
          </Col>
        </Row>
      </Form>
    </Col>
  </>
)

export default Home;
