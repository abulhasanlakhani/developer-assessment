import { render, screen } from '@testing-library/react'
import TodoList from './TodoList'

const todos = [
  { id: '5660e7b9-7555-4d3f-b863-df658440820b', description: 'Todo 1', isCompleted: false },
  { id: 'cbab58bb-fa24-46b9-b68d-ee25ddefb1a6', description: 'Todo 2', isCompleted: false },
  { id: 'bcb81fd8-ab1d-4874-af23-35513d3d673d', description: 'Todo 3', isCompleted: false },
]

describe('Verify number of Todos', () => {
  test('Todo list shows three items', async () => {
    render(<TodoList items={todos} />)

    expect(await screen.findByText('Todo 1')).toBeVisible()
    expect(await screen.findByText('Todo 2')).toBeVisible()
    expect(await screen.findByText('Todo 3')).toBeVisible()
    expect(await screen.findByText('Showing 3 Item(s)')).toBeVisible()
  })

  test('Todo list doesn\'t show the todo table and the heading', async () => {
    render(<TodoList items={[]} />)

    expect(screen.queryByRole('heading')).toBeNull()
    expect(screen.queryByRole('table')).toBeNull()
  })
})