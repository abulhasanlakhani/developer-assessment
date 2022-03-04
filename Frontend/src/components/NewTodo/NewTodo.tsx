import './NewTodo.css'
import React, { useEffect, useState, useRef, MouseEvent } from 'react'
import { Button, Container, Row, Col, Form, Stack } from 'react-bootstrap'
import axios from 'axios'
import { TodoItemType } from '../TodoItem/TodoItem'

export type NewTodoProps = {
  todoItems: TodoItemType[]
  setTodoItems: React.Dispatch<React.SetStateAction<TodoItemType[]>>
}

const NewTodo = (newTodoProps: NewTodoProps) => {
  const API_URL = 'https://localhost:5001/api/todoitems'
  const descriptionRef = useRef<HTMLInputElement>(null)
  const errRef = useRef<HTMLParagraphElement>(null)

  const [description, setDescription] = useState('')
  const [validDescription, setValidDescription] = useState(false)

  const [errMsg, setErrMsg] = useState('')

  const validateDescription = () => {
    if (description === '') {
      setErrMsg('Description is required')
      setValidDescription(false)
      return false
    }

    if (newTodoProps.todoItems.find((i) => i.description === description)) {
      setErrMsg('Description already exists')
      setValidDescription(false)
      return false
    }

    setValidDescription(true)
    return true
  }

  // focus on description textbox when page is loaded
  useEffect(() => {
    descriptionRef.current?.focus()
  }, []) // empty array here means it only runs on the initial page load

  // Clear any error as soon as user starts typing again
  useEffect(() => {
    setErrMsg('')
  }, [description]) // passing description here means this hook will only run if description state is changed

  const handleSubmit = async (e: MouseEvent<HTMLButtonElement>) => {
    e.preventDefault()

    if (!validateDescription()) return

    try {
      const response = await axios.post(API_URL, {
        description: description,
      })

      if (response?.status === 201) {
        let updatedTodos = [...newTodoProps.todoItems]
        updatedTodos.push(response.data)
        newTodoProps.setTodoItems(updatedTodos)
      }

      //clear state and controlled inputs
      //need value attrib on inputs for this
      setDescription('')
    } catch (err) {
      // Credit: https://codesandbox.io/s/upbeat-easley-ljgir?file=/src/index.ts:134-180
      if (axios.isAxiosError(err) && err.response) {
        if (!err?.response) {
          setErrMsg('No Server Response')
        } else if (err.response?.status === 400) {
          setErrMsg(err?.response.data)
        } else {
          setErrMsg('Add failed')
        }
      }

      errRef.current?.focus()
    }
  }

  const handleClear = () => {
    setDescription('')
  }

  return (
    <Container>
      <h1>Add New Todo</h1>
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
          />
        </Col>
      </Form.Group>
      <Form.Group as={Row} className="mb-3 offset-md-2" controlId="formAddTodoItem">
        <Stack direction="horizontal" gap={2}>
          <Button variant="primary" onClick={handleSubmit} name="btnAddTodoItem">
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
