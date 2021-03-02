import React, { useState, useEffect } from 'react'
import firebase from '../services/firebase'
import { Button, Form } from 'react-bootstrap'

export default function Quiz() {
    // Declare a new state variable, which we'll call "count"
    const [dataHasBeenFetched, setFetched] = useState(false)
    const ref = firebase.database().ref("Location")
    const [locations, setLocations] = useState({})
    const [locationDropDown, setLocDropDown] = useState([<option key='0'>Select location</option>])
    const [selectedLocation, setSelectedLocation] = useState(undefined)
    const [showForm, setShowForm] = useState(false)
    const [theObject, setTheObject] = useState({
        q1: {
            q: 'Question 1', 
            a1: 'Answer Option 1',
            a2: 'Answer Option 2',
            a3: 'Answer Option 3',
            r: 1 // right answer
        },
        q2: {
            q: 'Question 2', 
            a1: 'Q2Answer Option 1',
            a2: 'Q2Answer Option 2',
            a3: 'Q2Answer Option 3',
            r: 3
        },
        q3: {
            q: 'Question 3', 
            a1: 'Q3Answer Option 1',
            a2: 'Q3Answer Option 2',
            a3: 'Q3Answer Option 3',
            r: 2
        }
    })

    useEffect(() => {
        if (!dataHasBeenFetched) {
            ref.on('value', (snapshot) => {
                if (snapshot.val() !== null) {
                    setLocations(snapshot.val())
                    setLocDropDown([...locationDropDown, Object.entries(snapshot.val()).map(([key, value]) => {
                        return <option key={value.id}>{key}, {value.id}</option>
                    })])
                    setFetched(true)
                } else {
                    console.log('no locations found')
                }
            })
        }
    })

    const handleSubmit = (event) => {
        event.preventDefault()
        console.log(selectedLocation)
        const locId = selectedLocation.substring(selectedLocation.length - 13)

        const loc = Object.entries(locations).find(([key,value]) => {
            return (
                value.id === parseInt(locId)
            )
        })
        console.log(loc)
        ref.child(loc[0]).child('quiz').set(theObject)
    }

    const handleChange = (e) => {
        console.log(e.target.name)
        let tmpObj = {...theObject}
        switch(e.target.name) {
            case 'location':
                setSelectedLocation(e.target.value)
                setShowForm(true)
                break
            case 'q1':
                tmpObj.q1.q = e.target.value
                break
            case 'q2':
                tmpObj.q2.q = e.target.value
                break
            case 'q3':
                tmpObj.q3.q = e.target.value
                break
            case 'q1a1':
                tmpObj.q1.a1 = e.target.value
                break
            case 'q1a2':
                tmpObj.q1.a2 = e.target.value
                break
            case 'q1a3':
                tmpObj.q1.a3 = e.target.value
                break
            case 'q2a1':
                tmpObj.q2.a1 = e.target.value
                break
            case 'q2a2':
                tmpObj.q2.a2 = e.target.value
                break
            case 'q2a3':
                tmpObj.q2.a3 = e.target.value
                break
            case 'q3a1':
                tmpObj.q3.a1 = e.target.value
                break
            case 'q3a2':
                tmpObj.q3.a2 = e.target.value
                break
            case 'q3a3':
                tmpObj.q3.a3 = e.target.value
                break
            default:
                console.log('switch error:', e.target.name)
        }
        setTheObject(tmpObj)
    }

    const handleRadio = (e) => {
        let tmpObj = {...theObject}
        switch(e.target.name) {
            case '0':
                tmpObj.q1.r = parseInt(e.target.value)
                break
            case '1':
                tmpObj.q2.r = parseInt(e.target.value)
                break
            case '2':
                tmpObj.q3.r = parseInt(e.target.value)
                break
            default:
                console.log('error')
        }
        setTheObject(tmpObj)
    }
    
    /*
    const renderQuizTable = () =>  {
        return Object.entries(locations).map(([key, value], index) => {
            return (
                <tr key={index}>
                    <td>{value.a}</td>
                    <td>{value.b}</td>
                </tr>
                
            )
        })
    }
    */
  
    return (
      <div>
        <Form onSubmit={handleSubmit}>
            <Form.Control as="select" custom name="location" value={selectedLocation} onChange={handleChange}>
                {locationDropDown}
            </Form.Control>
            {showForm ? 
            <div>
            <h2>{selectedLocation}</h2>

            <Form.Group>
                <Form.Label htmlFor="q1">Question 1:</Form.Label>
                <Form.Control type="text" name="q1" onChange={handleChange} />
                <Form.Label htmlFor="q1a1">Answer 1:</Form.Label>
                <Form.Control type="text" name="q1a1" onChange={handleChange} />
                <Form.Label htmlFor="q1a2">Answer 2:</Form.Label>
                <Form.Control type="text" name="q1a2" onChange={handleChange} />
                <Form.Label htmlFor="q1a3">Answer 3:</Form.Label>
                <Form.Control type="text" name="q1a3" onChange={handleChange} />
                <Form.Check inline label="1" value="1" name="0" type='radio' onChange={handleRadio} checked={theObject.q1.r === 1}/>
                <Form.Check inline label="2" value="2" name="0" type='radio' onChange={handleRadio} checked={theObject.q1.r === 2} />
                <Form.Check inline label="3" value="3" name="0" type='radio' onChange={handleRadio} checked={theObject.q1.r === 3} />
            </Form.Group>

            <Form.Group>
                <Form.Label htmlFor="q2">Question 2:</Form.Label>
                <Form.Control type="text" name="q2" onChange={handleChange} />
                <Form.Label htmlFor="q2a1">Answer 1:</Form.Label>
                <Form.Control type="text" name="q2a1" onChange={handleChange} />
                <Form.Label htmlFor="q2a2">Answer 2:</Form.Label>
                <Form.Control type="text" name="q2a2" onChange={handleChange} />
                <Form.Label htmlFor="q2a3">Answer 3:</Form.Label>
                <Form.Control type="text" name="q2a3" onChange={handleChange} />
                <Form.Check inline label="1" value="1" name="1" type='radio' onChange={handleRadio} checked={theObject.q2.r === 1}/>
                <Form.Check inline label="2" value="2" name="1" type='radio' onChange={handleRadio} checked={theObject.q2.r === 2} />
                <Form.Check inline label="3" value="3" name="1" type='radio' onChange={handleRadio} checked={theObject.q2.r === 3} />
            </Form.Group>

            <Form.Group>
                <Form.Label htmlFor="q3">Question 3:</Form.Label>
                <Form.Control type="text" name="q3" onChange={handleChange} />
                <Form.Label htmlFor="q3a1">Answer 1:</Form.Label>
                <Form.Control type="text" name="q3a1" onChange={handleChange} />
                <Form.Label htmlFor="q3a2">Answer 2:</Form.Label>
                <Form.Control type="text" name="q3a2" onChange={handleChange} />
                <Form.Label htmlFor="q3a3">Answer 3:</Form.Label>
                <Form.Control type="text" name="q3a3" onChange={handleChange} />
                <Form.Check inline label="1" value="1" name="2" type='radio' onChange={handleRadio} checked={theObject.q3.r === 1}/>
                <Form.Check inline label="2" value="2" name="2" type='radio' onChange={handleRadio} checked={theObject.q3.r === 2} />
                <Form.Check inline label="3" value="3" name="2" type='radio' onChange={handleRadio} checked={theObject.q3.r === 3} />
            </Form.Group>



            <Form.Group>
                <Button variant="primary" type="submit">Submit</Button>
            </Form.Group>
            </div>
            : ''}
        </Form>
        {JSON.stringify(theObject, null, <br/>)}
      </div>
    );
  }