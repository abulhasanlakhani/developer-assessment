import React from 'react'
import { Button } from 'react-bootstrap'

const TodoItem = ({item, handleMarkAsComplete}) => {
    return (
    <tr role="row" style={{ textDecoration: item.isCompleted ? "line-through" : "" }}>
      <td role="cell">{item.id}</td>
      <td role="cell">{item.description}</td>
      <td role="cell">
        <Button variant="warning" size="sm" onClick={() => handleMarkAsComplete(item)}>
          {item.isCompleted && "Mark as in progress"} 
          {!item.isCompleted && "Mark as completed"} 
        </Button>
      </td>
    </tr>
  )
}

export default TodoItem
