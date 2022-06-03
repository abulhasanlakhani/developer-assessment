import { Badge, Button, ListGroup, Table } from 'react-bootstrap'
import TodoItem, { TodoItemType } from '../TodoItem/TodoItem'

export type TodoListType = {
  items: TodoItemType[]
  handleMarkAsComplete(item: TodoItemType): void
}

const TodoList = ({ items, handleMarkAsComplete }: TodoListType) => {
  return items.length ? (
    <>
      <h1>
        Showing {items.length} Item(s){' '}
        <Button variant="primary" className="pull-right">
          Refresh
        </Button>
      </h1>

      <ListGroup as="ul">
        {items.map((item, index) => {
          return (
            <ListGroup.Item as="li" className="d-flex justify-content-between align-items-start">
              <div className="ms-2 me-auto">
                <div className="fw-bold">{item.id}</div>
                {item.description}
              </div>
              <Badge bg={item.isCompleted ? 'success' : 'warning'} pill>
                {item.isCompleted ? 'Completed' : 'In Progress'}
              </Badge>
            </ListGroup.Item>
          )
        })}
      </ListGroup>

      <Table role="table" aria-label="todoList-table" striped bordered hover>
        <thead>
          <tr role="row">
            <th role="columnheader">Id</th>
            <th role="columnheader">Description</th>
            <th role="columnheader">Action</th>
          </tr>
        </thead>
        <tbody>
          {items.map((item, index) => {
            return (
              <TodoItem
                key={index}
                id={item.id}
                description={item.description}
                isCompleted={item.isCompleted}
                handleMarkAsComplete={handleMarkAsComplete}
              />
            )
          })}
        </tbody>
      </Table>
    </>
  ) : null
}

export default TodoList
