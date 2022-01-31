import { render, screen, queryByAttribute, fireEvent } from "@testing-library/react"
import App from "../../App"
import NewTodo from "./NewTodo"

test('Validation errors are not visible by default', async () => {
    render(<App />)
        
    expect(await screen.findByRole('button', { name: /Add Item/i})).toBeEnabled()

    expect(screen.queryByText('Description is required')).toBeNull()   
})

test('Description required error message is visible if ', async () => {
    const todoComp = render(<NewTodo />)
    const addTodoButton = todoComp.container.querySelector("button[name='btnAddTodoItem']")
        
    expect(await screen.findByRole('button', { name: /Add Item/i})).toBeEnabled()
    fireEvent.click(addTodoButton)

    screen.getByText('Description is required')   
})