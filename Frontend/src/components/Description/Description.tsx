import { Alert, Image } from 'react-bootstrap'
import { vegefruit } from '../../Models/viewmodels'

const Description = () => {
  return (
    <Alert variant="success" style={{ marginBottom: 0 }}>
      <Alert.Heading>Todo List App</Alert.Heading>
      <p>Todos on steroids</p>
    </Alert>
  )
}

export default Description
