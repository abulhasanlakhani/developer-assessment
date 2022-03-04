import { render, screen } from '@testing-library/react'
import React from 'react'
import App from './App'
import '@testing-library/jest-dom'

test('renders the footer text', async () => {
  jest.spyOn(React, 'useEffect').mockImplementation((f) => f())

  render(<App isTest={true} />)
  const footerElement = await screen.findByText(/clearpoint.digital/i)
  expect(footerElement).toBeInTheDocument()
})
