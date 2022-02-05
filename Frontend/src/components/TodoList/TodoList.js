import React from 'react';
import { Button, Table } from 'react-bootstrap'
import TodoItem from '../TodoItem/TodoItem';

const TodoList = ({items, handleMarkAsComplete}) => {
    return (
        items.length ? <>
          <h1 role="heading">
            Showing {items.length} Item(s){' '}
            <Button variant="primary" className="pull-right">
              Refresh
            </Button>
          </h1>
  
          <Table role="table" aria-label='todoList-table' striped bordered hover>
            <thead>
              <tr role="row">
                <th role="columnheader">Id</th>
                <th role="columnheader">Description</th>
                <th role="columnheader">Action</th>
              </tr>
            </thead>
            <tbody>
              {items.map((item, index) => (
                <TodoItem key={index} item={item} handleMarkAsComplete={handleMarkAsComplete} />
              ))}
            </tbody>
          </Table>
        </> : null
      )
};

export default TodoList;
