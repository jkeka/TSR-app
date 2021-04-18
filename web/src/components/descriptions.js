/* eslint-disable react-hooks/exhaustive-deps */
import React, { useState, useEffect } from 'react'
import firebase from '../services/firebase'
import { Button, Form } from 'react-bootstrap'

const shipTemplate = {
  owner: '',
  builder: '',
  launched: 0,
  description: '',
  length: 0,
  beam: 0,
  height: 0,
  depth: 0,
  draft: 0,
  tonnage:0,
  status: '',
  speed: 0,
  shiptype: ''
}
const boothTemplate = {
  description: '',
  another_field: ''
}

const defaultTypes = ['ship', 'booth', 'venue', 'restaurant']

export default function Descriptions() {
  const ref = firebase.database().ref()
  const [allLocations, setAllLocations] = useState({})
  const [locations, setLocations] = useState({default: {id: ''}})
  const [AllLocationsChanged, setAllLocationsChanged] = useState(false)
  const [SelectedTypeChanged, setSelectedTypeChanged] = useState(false)

  const [locationDropDown, setLocDropDown] = useState([<option key='0'>Select location</option>])
  const [selectedLocation, setSelectedLocation] = useState(undefined)
  const [theLocation, setTheLocation] = useState(undefined)

  const typeDropDown = defaultTypes.map(type => <option key={type}>{type}</option>)
  const [selectedType, setType] = useState(defaultTypes[0])

  const [descriptions, setDescriptions] = useState({})

  const [inputList, setInputList] = useState([])

  const [langSelected, setLanguage] = useState('en')
  const langDropDown = ['en', 'fi', 'se'].map(l => <option key={l}>{l}</option>)
  const [showForm, setShowForm] = useState(false)
  const [theObject, setTheObject] = useState({})
  let copy = {}
  const [theTemplate, setTheTemplate] = useState(shipTemplate)

  const handleChange = (e) => {
    console.log(e.target.name)
    switch(e.target.name) {
      case 'location':
        const locId = parseInt(e.target.value.substring(e.target.value.length - 13))
        let searchedLoc = {}
        Object.entries(locations).forEach(loc => {
          if(loc[1].id === locId) {
            searchedLoc = locations[loc[0]]
          }
        })

        console.log(searchedLoc)
        setTheLocation(searchedLoc)
        setSelectedLocation(e.target.value)
        const theLocData = Object.entries(descriptions).find(([key, value]) => parseInt(key) === locId)
        if (theLocData && theLocData[1][langSelected]) {
          copy = theLocData[1][langSelected]
          setTheObject(theLocData[1][langSelected])
        }
        setShowForm(true)
        break
      case 'type':
        setType(e.target.value)
        setSelectedTypeChanged(!SelectedTypeChanged)
        break
      case 'language':
        const theLocDat = Object.entries(descriptions).find(([key, value]) => parseInt(key) === theLocation.id)
        let langValue = e.target.value.slice(0)
        if (theLocDat && theLocDat[1][langValue]) {
          copy = theLocDat[1][langValue]
          setTheObject(theLocDat[1][langValue])
        }
        setLanguage(e.target.value)
        break
      default:
        let tmpObj = {...copy}
        let target = e.target.name.slice(0)
        let value = e.target.value.slice(0)
        tmpObj[target] = value
        copy = tmpObj
        console.log(copy, tmpObj)
        setTheObject(copy)
    }
  }

  // Fetches data when component mounts
  useEffect(() => {
    ref.child("Location").once('value', (snapshot) => {
      if (snapshot.val() !== null) {
        console.log('locations found')
        setAllLocations(snapshot.val())
        setAllLocationsChanged(true)
      } else {
        console.log('no locations found')
      }
    })
  }, [ref])

  // Filters the data based on type selected
  useEffect(() => {
    let locs = Object.fromEntries(Object.entries(allLocations)
      .filter(([key, value]) => value.type === selectedType))
    console.log(locs)
    setLocations(locs)
    setSelectedTypeChanged(!setSelectedTypeChanged)
  }, [AllLocationsChanged, selectedType])

  // Sets the dropdown menu for different locations based on type
  useEffect(() => {
    setLocDropDown([<option key='0'>Select location</option>, Object.entries(locations).map(([key, value]) => {
      return <option key={value.id}>{key}, {value.id}</option>
    })])
  }, [SelectedTypeChanged, locations])

  useEffect(() => {
    if (selectedType === 'ship') {
      setTheTemplate(shipTemplate)
    } else {
      setTheTemplate(boothTemplate)
    }
  }, [selectedType, langSelected])

  useEffect(() => {
    console.log('INPUTS CHANGING')
    setInputList(Object.keys(theTemplate).map(key => {
      if (key === 'description') {
        return (
          <div key={key}>
            <label>{key}</label>
            <textarea name={key} onChange={handleChange} value={theObject[key]} rows="10" cols="50" />
          </div>
        )
      } else {
        return null/*(
          <div key={key}>
            <label>{key}</label>
            <input type="text" name={key} onChange={handleChange} value={theObject[key]} />
          </div>
        )
      */}
        
    }))
  }, [JSON.stringify(theTemplate), langSelected])

  useEffect(() => {
    console.log('THE OBJECT CHANGING')
  }, [JSON.stringify(theObject)])

  useEffect(() => {
      ref.child("Descriptions").once('value', (snapshot) => {
        if (snapshot.val() !== null) {
          console.log(snapshot.val())
          setDescriptions(snapshot.val())
        } else {
          console.log('no descriptions found')
        }
      })
  }, [JSON.stringify(descriptions), ref])


  const handleSubmit = (e) => {
    e.preventDefault()
    console.log(theLocation)
    console.log(langSelected)
    ref.child('Descriptions').child(theLocation.id).child(langSelected).set(theObject)
    let tmpDescriptions = {...descriptions}
    tmpDescriptions[theLocation.id][langSelected] = theObject
    setDescriptions(tmpDescriptions)
  }

  return (
    <div>
      <h1>Descriptions</h1>
      <p>Object to be sent to DB under {theLocation !== undefined ? theLocation.id : 'not chosen'} {'->'} {langSelected}:<br/>
        {JSON.stringify(theObject)}</p>
      <Form onSubmit={handleSubmit}>
        <Form.Control as="select" custom name="type" value={selectedType} onChange={handleChange}>
            {typeDropDown}
        </Form.Control>
        <Form.Control as="select" custom name="location" value={selectedLocation} onChange={handleChange}>
            {locationDropDown}
        </Form.Control>
        {showForm ? 
        <div>
        <h2>{theLocation.name}</h2>

        <Form.Group>
          <Form.Control as="select" custom name="language" value={langSelected} onChange={handleChange}>
            {langDropDown}
          </Form.Control>
        </Form.Group>

        <Form.Group>
          {inputList}
        </Form.Group>

        <Form.Group>
          <Button variant="primary" type="submit">Submit</Button>
        </Form.Group>
        </div>
        : 'Select a location'}
      </Form>
    </div>
  )
}