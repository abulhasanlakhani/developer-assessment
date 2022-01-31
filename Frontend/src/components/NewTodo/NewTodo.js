import './NewTodo.css'
import React, { useEffect, useState, useRef } from 'react'
import { Button, Container, Row, Col, Form, Stack } from 'react-bootstrap'
import axios from 'axios'

const NewTodo = ({ todoItems, setTodoItems }) => {
  const API_URL = 'https://localhost:5001/api/todoitems'
  const descriptionRef = useRef()
  const errRef = useRef()

  const [description, setDescription] = useState('')
  const [validDescription, setValidDescription] = useState(false)
  const [, setDescriptionFocus] = useState(false)
  const [, setValidated] = useState(false)

  const [errMsg, setErrMsg] = useState('')

  const validateDescription = () => {
    
    if (description === '') {
      setErrMsg('Description is required')
      setValidDescription(false)
      return false
    }

    if (todoItems.find((i) => i.description === description)) {
      setErrMsg('Description already exists')
      setValidDescription(false)
      return false
    }

    setValidDescription(true)
    return true
  }

  useEffect(() => {
    descriptionRef.current.focus()
  }, [])

  useEffect(() => {
    setErrMsg('')
  }, [description])

  const handleSubmit = async (e) => {
    e.preventDefault()

    if (!validateDescription())
      return

    try {
      const response = await axios.post(API_URL, {
        description: description,
      })

      if (response?.status === 201) {
        console.log(response)

        let updatedTodos = [...todoItems]
        updatedTodos.push(response.data)
        setTodoItems(updatedTodos)
      }

      //clear state and controlled inputs
      //need value attrib on inputs for this
      setDescription('')
      setValidated(true)
    } catch (err) {
      if (!err?.response) {
        setErrMsg('No Server Response')
      } else if (err.response?.status === 400) {
        setErrMsg(err?.response.data)
      } else {
        setErrMsg('Add failed')
      }
      errRef.current.focus()
    }
  }

  const handleClear = () => {
    setDescription('')
  }

  return (
    <Container>
      <h1>Add Item</h1>
      <p ref={errRef} className={errMsg ? 'errmsg' : 'offscreen'} aria-live="assertive">
        {errMsg}
      </p>
      <Form.Group as={Row} className="mb-3" controlId="formAddTodoItem">
        <Form.Label column sm="2">
          Description
        </Form.Label>
        <Col md="6">
          <Form.Control
            type="text"
            placeholder="Enter description..."
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            ref={descriptionRef}
            required
            aria-invalid={validDescription ? 'false' : 'true'}
            onFocus={() => setDescriptionFocus(true)}
            onBlur={() => setDescriptionFocus(false)}
          />
        </Col>
      </Form.Group>
      <Form.Group as={Row} className="mb-3 offset-md-2" controlId="formAddTodoItem">
        <Stack direction="horizontal" gap={2}>
          <Button variant="primary" onClick={handleSubmit} name='btnAddTodoItem'>
            Add Item
          </Button>
          <Button variant="secondary" onClick={handleClear}>
            Clear
          </Button>
        </Stack>
      </Form.Group>
    </Container>
  )
}

export default NewTodo
