import React, { useRef } from 'react';
import { Button, Container, Row, Col, Form, Stack } from 'react-bootstrap'

const NewTodo = ({description, handleDescriptionChange, handleAdd, handleClear}) => {
    const todoInputRef = useRef(null);
    
    const submitHandler = (event) => {
        const enteredDescription = todoInputRef.current.value

        if (enteredDescription.trim().length === 0) {
            // throw an error
            return;
        }

        handleAdd(enteredDescription)
    }

    return (
        <Container>
          <h1>Add Item</h1>
          <Form.Group as={Row} className="mb-3" controlId="formAddTodoItem">
            <Form.Label column sm="2">
              Description
            </Form.Label>
            <Col md="6">
              <Form.Control
                type="text"
                placeholder="Enter description..."
                value={description}
                onChange={handleDescriptionChange}
                ref={todoInputRef}
              />
            </Col>
          </Form.Group>
          <Form.Group as={Row} className="mb-3 offset-md-2" controlId="formAddTodoItem">
            <Stack direction="horizontal" gap={2}>
              <Button variant="primary" onClick={(e) => submitHandler(e)}>
                Add Item
              </Button>
              <Button variant="secondary" onClick={() => handleClear()}>
                Clear
              </Button>
            </Stack>
          </Form.Group>
        </Container>
      )
};

export default NewTodo;
