/* eslint-disable react-hooks/exhaustive-deps */
import React, { Component } from 'react'
import firebase from '../services/firebase'
import { Button } from 'react-bootstrap'
import SHIPDATA from './data/shipdata'
import createTemplates from './descriptions/createTemplates'

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

    if (shipData[1] && shipData[1].data) {
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
    }
    

    return preview
  }
  createTemplate() {
    console.log('click')
    const shipRef = Object.entries(this.state.descriptions).find((key, value) => parseInt(key) === this.state.locationSelected)
    let s = JSON.parse(JSON.stringify(shipRef))[1].data

    let template = ''
    if (this.state.selectedLanguage === 'fi') {
      if (s.year && s.name && s.country) {
        template += '##name## rakennettiin vuonna ##year##. Sen kotimaa on ##country##. '
      }
      if (s.riki) {
        template += 'Sen rikityyppi on ##riki##. '
      }
      if (s.crew) {
        template += 'Aluksessa on ##crew## henkilön miehistö. '
      }
      if (s.length || s.height || s.width || s.depth) {
        template += '\n\n'
        if (s.length) {
          template += '- pituus: ##length##\n'
        }
        if (s.height) {
          template += '- korkeus: ##height##\n'
        }
        if (s.width) {
          template += '- leveys: ##width##\n'
        }
        if (s.depth) {
          template += '- syvyys: ##depth##\n'
        }
      }
      const preview = this.getPreview(template)
      this.setState({description: template, preview: preview})
    } else if (this.state.selectedLanguage === 'en') {
      if (s.year && s.name && s.country) {
        template += '##name## was built in ##year##. It sails under the flag of ##country##. '
      }
      if (s.riki) {
        template += `It's type of rigging is ##riki##.`
      }
      if (s.crew) {
        template += 'The ship has a crew of ##crew## members. '
      }
      if (s.length || s.height || s.width || s.depth) {
        template += '\n\n'
        if (s.length) {
          template += '- length: ##length##\n'
        }
        if (s.height) {
          template += '- height: ##height##\n'
        }
        if (s.width) {
          template += '- width: ##width##\n'
        }
        if (s.depth) {
          template += '- depth: ##depth##\n'
        }
      }
      const preview = this.getPreview(template)
      this.setState({description: template, preview: preview})
    } else if (this.state.selectedLanguage === 'se') {
      console.log('seclick')
      if (s.year && s.name && s.country) {
        template += '##name## byggdes ##year##. Det seglar under ##country##s flagga. '
      }
      if (s.riki) {
        template += `Det är en typ av rigg som är ##riki##.`
      }
      if (s.crew) {
        template += 'Fartyget har en besättning på ##crew## medlemmar. '
      }
      if (s.length || s.height || s.width || s.depth) {
        template += '\n\n'
        if (s.length) {
          template += '- längd: ##length##\n'
        }
        if (s.height) {
          template += '- höjd: ##height##\n'
        }
        if (s.width) {
          template += '- bredd: ##width##\n'
        }
        if (s.depth) {
          template += '- djup: ##depth##\n'
        }
      }
      const preview = this.getPreview(template)
      this.setState({description: template, preview: preview})
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
        .child(this.state.selectedLanguage).child('description').set(this.state.preview)
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
  createDescriptionsForAllShips() {
    if (window.confirm('Create descriptions for all ships? \n\n Warning! This will overwrite all existing descriptions!!!')) {
      let newData = createTemplates(this.state.descriptions)
      let combined = { ...this.state.descriptions, ...newData }
      console.log(combined)
      this.ref.child('Descriptions').set(combined)
      this.setState({descriptions: combined})
    }  
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
              return <li style={style} key={key}>{key}: {value}</li>
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
        
        {this.state.authed ?
        <div>
          <button onClick={() => this.theClick()}>update ship data</button>
          <button onClick={() => this.createDescriptionsForAllShips()}>
              create descriptions for all ships from templates
          </button>
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