import React, { Component } from 'react'
import { Button, Form, Table, Row, Col, Container } from 'react-bootstrap'

export default class User extends Component {
  constructor(props) {
    super(props)
    this.state = {
      deviceCode: '',
      language: '',
      rewardKey: [''],
      claimedRewardKey: [''], 
      tempDB: []
    }
    
    this.handleChange = this.handleChange.bind(this)
    this.handleSubmit = this.handleSubmit.bind(this)
    this.modifyClicked = this.modifyClicked.bind(this)
    this.addRewardKey = this.addRewardKey.bind(this)
    this.handleUpdate = this.handleUpdate.bind(this)
    this.removeDevice = this.removeDevice.bind(this)
    this.removeReward = this.removeReward.bind(this)
    this.removeClaimedReward = this.removeClaimedReward.bind(this)
  }
  componentDidMount() {
    this.setState({tempDB: [
      {
        deviceCode: '1',
        language: 'fi',
        rewardKey: ['xxx', 'yyy'],
        claimedRewardKey: ['xxx']
      },
      {
        deviceCode: '2',
        language: 'en',
        rewardKey: ['xxx', 'yyy', 'zzz'],
        claimedRewardKey: ['xxx', 'yyy']
      },
      {
        deviceCode: '3',
        language: 'se',
        rewardKey: ['aaa'],
        claimedRewardKey: ['aaa']
      }
    ]})
  }
  addRewardKey() {
    let rewardTable = [...this.state.rewardKey]
    rewardTable.push('')
    this.setState({rewardKey: rewardTable})
  }
  addClaimedRewardKey() {
    let claimedRewardTable = [...this.state.claimedRewardKey]
    claimedRewardTable.push('')
    this.setState({claimedRewardKey: claimedRewardTable})
  }
  modifyClicked(user) {
    this.setState({
      deviceCode: user.deviceCode,
      language: user.language,
      rewardKey: user.rewardKey,
      claimedRewardKey: user.claimedRewardKey
    })
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
    let db = [...this.state.tempDB]
    if (db.some(device => device.deviceCode === this.state.deviceCode)) {
      alert('device already exists')
    } else {
      db.push({
        deviceCode: this.state.deviceCode,
        language: this.state.language,
        rewardKey: this.state.rewardKey,
        claimedRewardKey: this.state.claimedRewardKey
      })
      this.setState({
        tempDB: db
      })
    }
    
    event.preventDefault()
  }
  handleUpdate() {
    let db = [...this.state.tempDB]
    let objToUpdate = db.find( ({deviceCode}) => deviceCode === this.state.deviceCode )
    if (objToUpdate) {
      objToUpdate.language = this.state.language
      objToUpdate.rewardKey = this.state.rewardKey
      objToUpdate.claimedRewardKey = this.state.claimedRewardKey
      this.setState({tempDB: db})
    } else {
      alert('no device found')
    }
    
  }
  removeDevice(device) {
    let db = [...this.state.tempDB]
    db = db.filter(dev => device.deviceCode !== dev.deviceCode)
    this.setState({tempDB: db})
  }
  removeReward(index) {
    let rewardTable = [...this.state.rewardKey]
    rewardTable.splice(index, 1)
    this.setState({rewardKey: rewardTable})
  }
  removeClaimedReward(index) {
    let claimedRewardTable = [...this.state.rewardKey]
    claimedRewardTable.splice(index, 1)
    this.setState({rewardKey: claimedRewardTable})
  }
  render() {
    const resultTable = this.state.tempDB.map(device => {
      return (
        <tr key={device.deviceCode}>
          <td>{device.deviceCode}</td>
          <td>{device.language}</td>
          <td>
            <ul>
              {device.rewardKey.map(key => <li key={key}>{key}</li>)}
            </ul>
          </td>
          <td>
            <ul>
              {device.claimedRewardKey.map(key => <li key={key}>{key}</li>)}
            </ul>
          </td>
          <td>
            <Button onClick={() => this.modifyClicked(device)}>Modify</Button>
            <Button variant="danger" onClick={() => this.removeDevice(device)}>Remove</Button>
          </td>
        </tr>
      )
    })
    const rewardKeyList = this.state.rewardKey.map((rewardKey, index) => {
      return (
        <Row key={index}>
          <Col sm={10}>
            <Form.Control type="text" name="rewardKey" value={rewardKey}
              onChange={(event) => this.handleChange(event, index)} />
          </Col>
          <Col sm={2}>
            <Button variant="danger" onClick={() => this.removeReward(index)}>Remove</Button>
          </Col>
        </Row>
      )
    })
    const claimedRewardKeyList = this.state.claimedRewardKey.map((claimedReward, index) => {
      return (
        <Row key={index}>
          <Col sm={10}>
            <Form.Control type="text" name="claimedRewardKey" value={claimedReward}
              onChange={(event) => this.handleChange(event, index)} />
          </Col>
          <Col sm={2}>
            <Button variant="danger" onClick={() => this.removeClaimedReward(index)}>Remove</Button>
          </Col>
        </Row>
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
            {rewardKeyList}
            <Button onClick={() => this.addRewardKey()}>+</Button><br/>
          </Form.Group>
          <Form.Group>
            <Form.Label htmlFor="claimedRewardKey">claimedRewardKey:</Form.Label>
            {claimedRewardKeyList}
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