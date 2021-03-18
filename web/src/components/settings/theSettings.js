import React, { useState, useEffect } from 'react'
import firebase from '../../services/firebase'
import DescSettings from './descSettings'

const defaultSettings = {
  events: {},
  user: {},
  location: {},
  quiz: {},
  reward: {},
  descriptions: { 
    testi: 'moi' 
  }
}

export default function Settings() {
  const ref = firebase.database().ref()
  const [allSettings, setAllSettings] = useState(defaultSettings)

  useEffect(() => {
    ref.child("Settings").once('value', (snapshot) => {
      if (snapshot.val() !== null) {
        console.log('settings found')
        setAllSettings(snapshot.val())
      } else {
        console.log('no settings found')
      }
    })
  }, [ref])



  return(
    <div>
      <h1>Settings</h1>
      <DescSettings data={allSettings.descriptions} />

    </div>
  )
}