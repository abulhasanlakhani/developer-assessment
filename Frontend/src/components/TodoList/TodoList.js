import React from 'react';
import { Button, Table } from 'react-bootstrap'
import TodoItem from '../TodoItem/TodoItem';

const TodoList = ({items, getItems, handleMarkAsComplete}) => {
    return (
        <>
          <h1>
            Showing {items.length} Item(s){' '}
            <Button variant="primary" className="pull-right" onClick={() => getItems()}>
              Refresh
            </Button>
          </h1>
  
          <Table striped bordered hover>
            <thead>
              <tr>
                <th>Id</th>
                <th>Description</th>
                <th>Action</th>
              </tr>
            </thead>
            <tbody>
              {items.map((item, index) => (
                <TodoItem key={index} item={item} handleMarkAsComplete={handleMarkAsComplete} />
              ))}
            </tbody>
          </Table>
        </>
      )
};

export default TodoList;
