import './App.css'
import { Container, Row, Col, Image } from 'react-bootstrap'
import React, { useState, useEffect } from 'react'
import axios from 'axios'
import TodoList from './components/TodoList/TodoList'
import NewTodo from './components/NewTodo/NewTodo'
import Footer from './components/Footer/Footer'
import Description from './components/Description/Description'

export const API_URL = 'https://localhost:5001/api/todoitems'

const App = () => {
  const [todoItems, setTodoItems] = useState([])

  useEffect(() => {
    // useEffect doesn't support async naturally so we have to first define async function and call it
    const loadTodosFromServer = async () => {
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
  }, []) // empty array here means this hook runs on the initial page load only

  const handleMarkAsComplete = async (item) => {
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
            <Image width="887px" height="212px" src="clearPointLogo.png" alt='Clearpoint logo' fluid rounded />
          </Col>
          <Col>
            <Description />
          </Col>
        </Row>
      </Container>
      <Container>
        <Row>
          <Col>
            <NewTodo todoItems={todoItems} setTodoItems={setTodoItems} />
          </Col>
        </Row>
        <br />
        <Row>
          <Col>
            <TodoList items={todoItems} handleMarkAsComplete={handleMarkAsComplete} />
          </Col>
        </Row>
      </Container>
      <Footer />
    </div>
  )
}

export default App
