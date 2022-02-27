import { render, screen } from '@testing-library/react'
import App from './App'

test('renders the footer text', async () => {
  render(<App />)
  const footerElement = await screen.findByText(/clearpoint.digital/i)
  expect(footerElement).toBeInTheDocument()
})
