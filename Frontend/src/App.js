import './App.css'
import { Container, Row, Col, Image } from 'react-bootstrap'
import React, { useState, useEffect } from 'react'
import axios from 'axios'
import TodoList from './components/TodoList/TodoList'
import NewTodo from './components/NewTodo/NewTodo'
import Footer from './components/Footer/Footer'
import Description from './components/Description/Description'

const App = () => {
  const [todoItems, setTodoItems] = useState([])
  const [, setSuccess] = useState(false)

  const API_URL = 'https://localhost:5001/api/todoitems'
  useEffect(() => {
    const loadTodosFromServer = async () => {
      console.log('Loading Todos from the server...')
      await axios
        .get(API_URL)
        .then((res) => {
          setTodoItems(res.data)
        })
        .catch((err) => {
          console.error('My Error: ', err)
        })
    }
    loadTodosFromServer()

    // Cleanup function - Set the success flag back to default position before this useEffect hook runs next time
    return function cleanup() {
      console.log('Running cleanup now...')
    }
  }, [])

  async function getItems() {
    try {
      alert('todo')
    } catch (error) {
      console.error(error)
    }
  }

  async function handleMarkAsComplete(item) {
    try {
      await axios
        .put(`${API_URL}/${item.id}`, {
          id: item.id,
          description: item.description,
          isCompleted: true,
        })
        .then(() => {
          let mapped = todoItems.map((i) => {
            return i.id === item.id ? { ...i, isCompleted: !i.isCompleted } : { ...i }
          })

          setTodoItems(mapped)
        })
        .catch((err) => {
          console.error(err)
        })
    } catch (error) {
      console.error(error)
    }
  }

  return (
    <div className="App">
      <Container style={{backgroundColor: 'var(--bs-green)', padding: '10px'}}>
        <Row>
          <Col xs={4} style={{ placeSelf: 'center' }}>
            <Image width={887} height={212} src="clearPointLogo.png" fluid rounded />
          </Col>
          <Col>
            <Description />
          </Col>
        </Row>
      </Container>
      <Container>
        <Row>
          <Col>
            <NewTodo todoItems={todoItems} setTodoItems={setTodoItems} successStatusHandler={setSuccess} />
          </Col>
        </Row>
        <br />
        <Row>
          <Col>
            <TodoList items={todoItems} getItems={getItems} handleMarkAsComplete={handleMarkAsComplete} />
          </Col>
        </Row>
      </Container>
      <Footer />
    </div>
  )
}

export default App
