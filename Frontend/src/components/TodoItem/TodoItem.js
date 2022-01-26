import React from 'react'
import { Button } from 'react-bootstrap'

const TodoItem = ({item, handleMarkAsComplete}) => {
    return (
    <tr>
      <td>{item.id}</td>
      <td>{item.description}</td>
      <td>
        <Button variant="warning" size="sm" onClick={() => handleMarkAsComplete(item)}>
          Mark as completed
        </Button>
      </td>
    </tr>
  )
}

export default TodoItem
