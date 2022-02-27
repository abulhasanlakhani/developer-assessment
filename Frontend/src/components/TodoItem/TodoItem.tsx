import { Button } from 'react-bootstrap'

export type TodoItemType = {
  id: number
  description: string
  isCompleted: boolean
  handleMarkAsComplete(item: TodoItemType): void
}

const TodoItem = (item: TodoItemType): JSX.Element => {
  return (
    <tr role="row" style={{ textDecoration: item.isCompleted ? 'line-through' : '' }}>
      <td role="cell">{item.id}</td>
      <td role="cell">{item.description}</td>
      <td role="cell">
        <Button variant="warning" size="sm" onClick={() => item.handleMarkAsComplete(item)}>
          {item.isCompleted && 'Mark as in progress'}
          {!item.isCompleted && 'Mark as completed'}
        </Button>
      </td>
    </tr>
  )
}

export default TodoItem
