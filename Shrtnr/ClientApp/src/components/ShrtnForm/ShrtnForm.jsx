import React from 'react';
import { Form, Input, Button, Col, Row, Spinner } from 'reactstrap';
import './ShrtnForm.css';

export const ShrtnForm = (props) => (
    <Form onSubmit={props.onSubmit}>
        <Row form>
          <Col md={11}>
            <Input 
                type="url"
                name="url"
                placeholder="https://www.example.com/looooooooooooong"
                bsSize="lg"
                required={true}
            />
          </Col>
          <Col md={1} className="shrtn-form-btn">
            <Button size="lg">{props.loading ? <Spinner size="sm" color="white" />: "Shrtn"}</Button>
          </Col>
        </Row>
      </Form>
);

export default ShrtnForm;