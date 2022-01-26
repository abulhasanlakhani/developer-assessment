import React from 'react'
import { Button } from 'react-bootstrap'

const TodoItem = ({item, handleMarkAsComplete}) => {
    return (
    <tr style={{ textDecoration: item.isCompleted ? "line-through" : "" }}>
      <td>{item.id}</td>
      <td>{item.description}</td>
      <td>
        <Button variant="warning" size="sm" onClick={() => handleMarkAsComplete(item)}>
          {item.isCompleted && "Mark as in progress"} 
          {!item.isCompleted && "Mark as completed"} 
        </Button>
      </td>
    </tr>
  )
}

export default TodoItem
