import { render, screen } from '@testing-library/react'
import { act } from 'react-dom/test-utils'
import App from './App'

test('renders the footer text', async () => {
  act(() => {
    render(<App />)
  })
  
  const footerElement = await screen.findByText(/clearpoint.digital/i)
  expect(footerElement).toBeInTheDocument()
})