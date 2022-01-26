import './App.css'
import { Image, Container, Row, Col } from 'react-bootstrap'
import React, { useState, useEffect } from 'react'
import axios from 'axios'
import TodoList from './components/TodoList/TodoList'
import NewTodo from './components/NewTodo/NewTodo'
import Description from './components/Description/Description'
import Footer from './components/Footer/Footer'

const App = () => {
  const [description, setDescription] = useState('')
  const [items, setItems] = useState([])
  const API_URL = 'https://localhost:5001/api/todoitems'
  useEffect(() => {
    const todoItems = async () => {
      // try {

      // } catch (error) {
      //   console.error('My Error: ', error)
      // }

      await axios
        .get(API_URL)
        .then((res) => {
          setItems(res.data)
          console.log(res.data)
        })
        .catch((err) => {
          console.error('My Error: ', err)
        })
      //setItems(data)
    }

    todoItems()
  }, [])

  const handleDescriptionChange = (event) => {
    setDescription(event.target.value)
  }

  async function getItems() {
    try {
      alert('todo')
    } catch (error) {
      console.error(error)
    }
  }

  async function handleAdd(enteredDescription) {
    try {
      const response = await axios
        .post(API_URL, {
          description: enteredDescription,
        })
        .catch((err) => {
          throw err
        })

      let updatedTodos = [...items]

      updatedTodos.push(response.data)

      setItems(updatedTodos)
    } catch (error) {
      console.error('Error has occured: ', error)
    }
  }

  function handleClear() {
    setDescription('')
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
          let mapped = items.map(i => {
            return i.id === item.id ? { ...i, isCompleted: !i.isCompleted } : { ...i};
          });
                    
          setItems(mapped)
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
      <Container>
        <Row>
          <Col>
            <Image src="clearPointLogo.png" fluid rounded />
          </Col>
        </Row>
        <Row>
          <Col>
            <Description />
          </Col>
        </Row>
        <Row>
          <Col>
            <NewTodo
              description={description}
              handleAdd={handleAdd}
              handleClear={handleClear}
              handleDescriptionChange={handleDescriptionChange}
            />
          </Col>
        </Row>
        <br />
        <Row>
          <Col>
            <TodoList items={items} getItems={getItems} handleMarkAsComplete={handleMarkAsComplete} />
          </Col>
        </Row>
      </Container>
      <Footer />
    </div>
  )
}

export default App
