import React from 'react'

export default function EventDescription({ desc, event, onChange, lang }) {
  let eventTitle, descriptionTitle
  switch (lang) {
    case 'fi':
      eventTitle = 'Tapahtuman nimi'
      descriptionTitle = 'Kuvaus suomeksi'
      break
    case 'se':
      eventTitle = 'Event namn'
      descriptionTitle = 'Beskrivning på svenska'
      break
    default:
      eventTitle = 'Event name'
      descriptionTitle = 'Description in English'
  }

  const handleChange = (e) => {
    let obj = { desc: desc, event: event }
    if (e.target.name.includes('Event')) {
      obj.event = e.target.value
    } else if (e.target.name.includes('Description')) {
      obj.desc = e.target.value
    }
    const x = { target: { name: lang } }
    onChange(x, obj)
  }

  const imgStyle = { position: 'relative', top: '-1em', right: '0', width: '10%', marginBottom: '-1em' } 

  return (
    <div width="100%">
      <img src={`/img/${lang}.png`} alt={lang} style={imgStyle} />
      <br/>
      {eventTitle}: <br/>
      <input name={`${lang}Event`} value={event} onChange={handleChange} style={{width: '100%'}} /> 
      <br/>
      {descriptionTitle}: <br/>
      <textarea name={`${lang}Description`} value={desc} onChange={handleChange} style={{width: '100%'}} />
    </div>
  )
} 