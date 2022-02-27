import { Button, Table } from 'react-bootstrap'
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
