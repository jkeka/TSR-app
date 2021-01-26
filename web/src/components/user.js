import React, { Component } from 'react'
import { Button, Form, Table, Row, Col, Container } from 'react-bootstrap'
import firebase from '../services/firebase'

export default class User extends Component {
  constructor(props) {
    super(props)
    this.ref = firebase.database().ref("Users")
    
    this.handleChange = this.handleChange.bind(this)
    this.handleSubmit = this.handleSubmit.bind(this)
    this.removeClaimedReward = this.removeClaimedReward.bind(this)
    this.state = {
      deviceCode: '',
      language: '',
      rewardKey: [''],
      claimedRewardKey: [''], 
      tempDB: []
    }
  }
  componentDidMount() {
    // Database fetch
    this.ref.on('value', (snapshot) => {
      console.log(snapshot.val())
      if (snapshot.val() !== null) {
        this.setState({tempDB: snapshot.val()})
      } else {
        this.setState({tempDB: [
          {
            deviceCode: '1',
            language: 'fi',
            rewardKey: ['xxx', 'yyy'],
            claimedRewardKey: ['xxx']
          }]})
      }
        
    })
  }
  addRewardKey() {
  
  }
  handleChange(event, index) {
    switch (event.target.name) {
      case 'deviceCode':
        this.setState({deviceCode: event.target.value})
        break;
      case 'language':
        this.setState({language: event.target.value})
        break;
      case 'rewardKey':
        let tmp = [...this.state.rewardKey]
        tmp[index] = event.target.value
        this.setState({rewardKey: tmp})
        break;
      case 'claimedRewardKey':
        let tmpClaimedReward = [...this.state.claimedRewardKey]
        tmpClaimedReward[index] = event.target.value
        this.setState({claimedRewardKey: tmpClaimedReward})
        break;
      default:
        console.log('error with switch')
    }
  }
  handleSubmit(event) {
    const testObj = { deviceCode: this.state.deviceCode, language: this.state.language }
    const ref = this.ref.child(this.state.deviceCode)
    ref.set(testObj)
    
    event.preventDefault() 
  }
  removeDevice(device) {
    console.log(device)
    const ref = this.ref.child(device)
    ref.remove()
  }
  removeReward(index) {

  }
  removeClaimedReward(key) {

  }
  render() {
    const resultTable = Object.entries(this.state.tempDB).map(([key, value], index) => {
      return (
        <tr key={index}>
          <td>{key}</td>
          <td>{value.language}</td>
          <td>
            <ul>
              
            </ul>
          </td>
          <td>
            <ul>
              
            </ul>
          </td>
          <td>
            <Button onClick={() => this.modifyClicked(key)}>Modify</Button>
            <Button variant="danger" onClick={() => this.removeDevice(key)}>Remove</Button>
          </td>
        </tr>
      )
    })
    return (
      <Container>

      <Form onSubmit={this.handleSubmit}>

        <Form.Group>
          <Form.Label htmlFor="device">deviceCode:</Form.Label>
          <Form.Control type="text" name="deviceCode" value={this.state.deviceCode}
            onChange={this.handleChange} />
        </Form.Group>

        <Form.Group>
          <Form.Label htmlFor="language">language:</Form.Label>
          <Form.Control type="text" name="language" value={this.state.language}
            onChange={this.handleChange} />
        </Form.Group>

        <Form.Group>
          <Form.Label htmlFor="rewardKey">rewardKey:</Form.Label>
          
          <Button onClick={() => this.addRewardKey()}>+</Button><br/>
        </Form.Group>
        <Form.Group>
          <Form.Label htmlFor="claimedRewardKey">claimedRewardKey:</Form.Label>
          
          <Button onClick={() => this.addClaimedRewardKey()}>+</Button><br/>
        </Form.Group>

        <Form.Group>
          <Button variant="primary" type="submit">Submit</Button>
          <Button variant="secondary" onClick={() => this.handleUpdate()}>Update</Button>
        </Form.Group>
      </Form>
    
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>device</th>
            <th>lang</th>
            <th>rewardKey</th>
            <th>claimedRewardKey</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {resultTable}
        </tbody>
      </Table>
    </Container>
    )
  }
}