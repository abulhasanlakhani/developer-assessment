import { render, screen, fireEvent } from '@testing-library/react'
import App from '../../App'
import NewTodo from './NewTodo'
import '@testing-library/jest-dom'
import React from 'react'

describe('NewTodoTests', () => {
  let useEffect: any

  const mockUseEffect = () => {
    useEffect.mockImplementationOnce((f: any) => f())
  }

  beforeEach(() => {
    useEffect = jest.spyOn(React, 'useEffect')
    mockUseEffect()
    mockUseEffect()
  })

  test('Validation errors are not visible by default', async () => {
    render(<App isTest={true} />)
    //isTest={true}
    expect(await screen.findByRole('button', { name: /Add Item/i })).toBeEnabled()

    expect(screen.queryByText('Description is required')).toBeNull()
  })

  test('Add item button is clicked without entering description', async () => {
    render(<NewTodo todoItems={[]} setTodoItems={() => {}} />)
    const addTodoButton = screen.getByText('Add Item')

    expect(await screen.findByRole('button', { name: /Add Item/i })).toBeEnabled()
    fireEvent.click(addTodoButton)

    expect(await screen.findByText('Description is required')).toBeVisible()
  })
})
