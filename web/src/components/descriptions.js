/* eslint-disable react-hooks/exhaustive-deps */
import React, { Component } from 'react'
import firebase from '../services/firebase'
import { Button } from 'react-bootstrap'
import SHIPDATA from './data/shipdata'

const defaultTypes = ['ship', 'booth', 'venue', 'restaurant']
const defaultLanguages = ['fi', 'en', 'se']

const countryFi = ['Venäjä','Suomi','Puola','UK','Espanja','Ranska','Tanska','Liettua','Norja','Saksa','Alankomaat','Viro','Belgia','Venäjä','Latvia','Ruotsi']
const countryEn = ['Russia','Finland','Poland','UK','Spain','France','Denmark','Lithuania','Norway','Germany','Netherlands','Estonia','Belgium','Russia','Latvia','Swedish']
const countrySe = ['Ryssland','Finland','Polen','Storbritannien','Spanien','Frankrike','Danmark','Litauen','Norge','Tyskland','Nederländerna','Estland','Belgien','Ryssland','Lettland','svenska']
const riggingFi = ['Sluuppi','Kahvelikuunari','Kutteri','Sluuppi','Ketsi','Huippupurje kuunari','Kahveliketsi','Täystakiloitu alus','Kuunari','Jooli','Riki','Kahvelikutteri','Barkentiini','Briki']
const riggingEn = ['Sloop','Fork schooner','Cutter','Sloop','Ketch','Top sail schooner','Fork chain','Fully weighted vessel','Schooner','Yawl','Rigging','Fork cutter','Barkentin','Rigging']
const riggingSe = ['Slup','Gaffelskonare','Fräs','Slup','Ketch','Topp segel skonare','Gaffelkedja','Fullt viktat fartyg','Skonare','Yawl','Tackling','Gaffelskärare','Barkentin','Tackling']

export default class Descriptions extends Component {
  constructor(props) {
    super(props)
    this.ref = firebase.database().ref()
    this.handleChange = this.handleChange.bind(this)
    this.findDescription = this.findDescription.bind(this)
    this.handleSubmit = this.handleSubmit.bind(this)
    this.state = {
      locations: {},
      descriptions: {},
      selectedType: defaultTypes[0],
      selectedLanguage: defaultLanguages[0],
      locationSelected: '',
      selectedLocations: {},
      description: '',
      showForm: false,
      authed: false,
      preview: ''
    }
  }
  componentDidMount() {
    this.ref.child("Location").once('value', (snapshot) => {
      if (snapshot.val() !== null) {
        console.log('locations found')
        this.setState({
          locations: snapshot.val(),
          selectedLocations: Object.values(snapshot.val()).filter((value) => value.type === this.state.selectedType),
          locationSelected: Object.values(snapshot.val()).filter((value) => value.type === this.state.selectedType)[0].id
        })
      } else {
        console.log('no locations found')
      }
    })
    this.ref.child("Descriptions").once('value', (snapshot) => {
      if (snapshot.val() !== null) {
        console.log('descriptions found')
        this.setState({descriptions: snapshot.val()})
        this.initializeDescription()
      } else {
        console.log('no descriptions found')
      }
    })
    const user = firebase.auth().currentUser;

    if (user) {
      this.setState({authed: true})
    } else {
      // No user is signed in.
    }
  }
  initializeDescription() {
    let desc = this.findDescription(this.state.selectedLanguage, this.state.locationSelected.toString())
    this.setState({description: desc, showForm: true})
  }
  handleChange(e) {
    switch (e.target.name) {
      case 'type':
        let desc3 = this.findDescription(this.state.selectedLanguage,
          Object.values(this.state.locations).filter((value) => value.type === e.target.value)[0].id,
          Object.values(this.state.locations).filter((value) => value.type === e.target.value)[0].id)
        this.setState({
          selectedType: e.target.value,
          selectedLocations: Object.values(this.state.locations).filter((value) => value.type === e.target.value),
          locationSelected: Object.values(this.state.locations).filter((value) => value.type === e.target.value)[0].id,
          description: desc3
        })
        break
      case 'location':
        console.log(e.target.value)
        const str = e.target.value
        const locId = str.substring(str.length - 14, str.length - 1)
        const newLoc = Object.values(this.state.locations).find((value) => value.id.toString() === locId)
        let desc2 = this.findDescription(this.state.selectedLanguage, newLoc.id.toString(), newLoc.id.toString())
        this.setState({
          locationSelected: newLoc.id,
          description: desc2
        })
        break
      case 'language':
        let desc = this.findDescription(e.target.value, this.state.locationSelected.toString())
        this.setState({selectedLanguage: e.target.value, description: desc})
        break
      case 'desc':
        let preview = this.getPreview(this.state.description.toString())

        this.setState({description: e.target.value, preview: preview})
        break
      default:
        console.log('error with switch')
    }
  }
  getPreview(preview) {
    const shipRef = Object.entries(this.state.descriptions).find((key, value) => parseInt(key) === this.state.locationSelected)
    let shipData = JSON.parse(JSON.stringify(shipRef))
    
    if (this.state.selectedLanguage === 'en') {
      let index = countryFi.findIndex(c => c === shipData[1].data.country)
      shipData[1].data.country = countryEn[index]
      index = riggingFi.findIndex(r => r === shipData[1].data.riki)
      shipData[1].data.riki = riggingEn[index]
    } else if (this.state.selectedLanguage === 'se') {
      let index = countryFi.findIndex(c => c === shipData[1].data.country)
      shipData[1].data.country = countrySe[index]
      index = riggingFi.findIndex(r => r === shipData[1].data.riki)
      shipData[1].data.riki = riggingSe[index]
    }

    Object.entries(shipData[1].data).forEach(([key, value]) => {
      if (value.toString().length < 1) {
        preview = preview.replaceAll(`##${key}##`, 'MISSING VALUE')
      } else {
        preview = preview.replaceAll(`##${key}##`, value.toString())
      }
    })

    return preview
  }
  createTemplate() {
    if (this.state.selectedLanguage === 'fi') {
      const fiTemplate = '##name## rakennettiin vuonna ##year##. Sen kotimaa on ##country##. Sen rikityyppi on ##riki##. Aluksessa on ##crew## henkilön miehistö. '
      const preview = this.getPreview(fiTemplate)
      this.setState({description: fiTemplate, preview: preview})
    } else if (this.state.selectedLanguage === 'en') {
      const enTemplate = `##name## was built in ##year##. It sails under the flag of ##country##. It's type of rigging is ##riki##. The ship has a crew of ##crew## members. `
      const preview = this.getPreview(enTemplate)
      this.setState({description: enTemplate, preview: preview})
    } else if (this.state.selectedLanguage === 'se') {

    }
  }
  findDescription(lang, key, location = this.state.locationSelected) {
    const desc = Object.entries(this.state.descriptions).find(([key, desc]) => {
      return key === location.toString()
    })
    let result = ''
    if (desc && desc[1][lang]) {
      result = desc[1][lang].description
    }
    return result
  }
  handleSubmit() {
    if (window.confirm("Submit the data?")) {

      console.log('Descriptions', this.state.locationSelected, this.state.selectedLanguage)
      this.ref.child('Descriptions').child(this.state.locationSelected)
        .child(this.state.selectedLanguage).child('description').set(this.state.description)
      let tmpDescriptions = {...this.state.descriptions}
      tmpDescriptions[this.state.locationSelected] = [this.state.selectedLanguage]
      tmpDescriptions[this.state.locationSelected][this.state.selectedLanguage] = this.state.description
      
      this.setState({descriptions: tmpDescriptions})
    }
    
  }
  theClick() {
    SHIPDATA.forEach(ship => {
      let match = Object.values(this.state.locations).find(loc => loc.shipId === ship.shipId)
      if (match) { 
        console.log(match)
        this.ref.child('Descriptions').child(match.id).child('data').set(ship)
      } else { 
        console.log('ship not in locations: ' + ship.name)
      }
      //console.log(match.name)
    })
    
  }
  render() {
    let shipData = ''
    if (this.state.selectedType === 'ship') {
      let desc = Object.entries(this.state.descriptions).find((key, value) => parseInt(key) === this.state.locationSelected)
      if (desc && desc[1].data) {
        // shipData = JSON.stringify(desc[1], null, 2)
        shipData = (
          <ul>
            {Object.entries(desc[1].data).map(([key, value]) => {
              const style = value.toString().length > 0 ? {color: 'green'} : {color: 'red'}
              return <li style={style}>{key}: {value}</li>
            })}
          </ul>
        )
      }
    }

    let descTable = Object.entries(this.state.descriptions).map(([key, value]) => {
      let loc = Object.values(this.state.locations).find(loc => loc.id.toString() === key.toString())
      if (!loc) {
        return <tr key={key}><td>{`error with ${key}`}</td></tr>
      }
      return (
        <tr key={key}>
          <td>{key}</td>
          <td>{loc.name}</td>
          <td>{loc.type}</td>
          <td>{value.fi ? 'Yes' : 'No'}</td>
          <td>{value.en ? 'Yes' : 'No'}</td>
          <td>{value.se ? 'Yes' : 'No'}</td>
        </tr>
      )
    })
    return (
      <div>
        <button onClick={() => this.theClick()}>update ship data</button>
        {this.state.authed ?
        <div>
          {this.state.showForm ?
          <div>
          <div style={{fontSize: "small"}}>
            <table>
              <thead>
                <tr>
                  <th>Id</th>
                  <th>Name</th>
                  <th>Type</th>
                  <th>Fi</th>
                  <th>En</th>
                  <th>Se</th>
                </tr>
              </thead>
              <tbody>
                {descTable}
              </tbody>
            </table>
          </div>

          <hr/>

          <select name="type" value={this.state.selectedType} onChange={this.handleChange}>
              {defaultTypes.map(type => <option key={type}>{type}</option>)}
          </select>
          
          {this.state.selectedLocations[0] ?
          <div>
            <select name="location" value={this.state.locationSelected.id} onChange={this.handleChange}>
                {this.state.selectedLocations.map(loc => <option key={loc.id}>{`${loc.name} (${loc.id})`}</option>)}
            </select>
            <select name="language" value={this.state.selectedLanguage} onChange={this.handleChange}>
                {defaultLanguages.map(lang => <option key={lang}>{lang}</option>)}
            </select>
          </div>
          : ''}
          <br/>
          {shipData}
          <button onClick={() => this.createTemplate()}>Template</button>
          <br/>
          {this.state.description !== '' ?
          <textarea name="desc" rows="10" cols="80" value={this.state.description} onChange={this.handleChange}/>
          :
          <textarea name="desc" rows="10" cols="60" value={this.state.description} 
            placeholder="No description found." onChange={this.handleChange}></textarea>
          }
          
          <br/>
          {this.state.preview.toString()}
          <br/>
          <Button onClick={() => this.handleSubmit()}>Submit</Button>

        </div>
        :
        <p>Loading...</p>
        }
      </div>
      :
      <p>Please log in</p>}
    </div>
      
    )
  }
}