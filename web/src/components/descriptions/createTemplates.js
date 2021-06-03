const countryFi = ['Venäjä','Suomi','Puola','UK','Espanja','Ranska','Tanska','Liettua','Norja','Saksa','Alankomaat','Viro','Belgia','Venäjä','Latvia','Ruotsi']
const countryEn = ['Russia','Finland','Poland','UK','Spain','France','Denmark','Lithuania','Norway','Germany','Netherlands','Estonia','Belgium','Russia','Latvia','Swedish']
const countrySe = ['Ryssland','Finland','Polen','Storbritannien','Spanien','Frankrike','Danmark','Litauen','Norge','Tyskland','Nederländerna','Estland','Belgien','Ryssland','Lettland','svenska']
const riggingFi = ['Sluuppi','Kahvelikuunari','Kutteri','Sluuppi','Ketsi','Huippupurje kuunari','Kahveliketsi','Täystakiloitu alus','Kuunari','Jooli','Riki','Kahvelikutteri','Barkentiini','Briki']
const riggingEn = ['Sloop','Fork schooner','Cutter','Sloop','Ketch','Top sail schooner','Fork chain','Fully weighted vessel','Schooner','Yawl','Rigging','Fork cutter','Barkentin','Rigging']
const riggingSe = ['Slup','Gaffelskonare','Fräs','Slup','Ketch','Topp segel skonare','Gaffelkedja','Fullt viktat fartyg','Skonare','Yawl','Tackling','Gaffelskärare','Barkentin','Tackling']

/**
 * This function automatically creates descriptions for ships.
 * 
 * If a value is missing, the line including it is skipped.
 * 
 * Translations in static tables have been made with Google Translate.
 */

const createTemplates = (descriptions) => {
  let newDescriptions = {}

  Object.entries(descriptions).forEach(([key, s]) => {
    if (s.data) {
      s = s.data
      let fiTemplate = ''
      let enTemplate = ''
      let seTemplate = ''
      if (s.year && s.name && s.country) {
        fiTemplate += '##name## rakennettiin vuonna ##year##. Sen kotimaa on ##country##. '
        enTemplate += '##name## was built in ##year##. It sails under the flag of ##country##. '
        seTemplate += '##name## byggdes ##year##. Det seglar under ##country##s flagga. '
      }
      if (s.riki) {
        fiTemplate += 'Sen rikityyppi on ##riki##. '
        enTemplate += `It's type of rigging is ##riki##. `
        seTemplate += `Det är en typ av rigg som är ##riki##. `
      }
      if (s.crew) {
        fiTemplate += 'Aluksessa on ##crew## henkilön miehistö. '
        enTemplate += 'The ship has a crew of ##crew## members. '
        seTemplate += 'Fartyget har en besättning på ##crew## medlemmar. '
      }
      if (s.length || s.height || s.width || s.depth) {
        fiTemplate += '\n\n'
        enTemplate += '\n\n'
        seTemplate += '\n\n'
        if (s.length) {
          fiTemplate += '- pituus: ##length##\n'
          enTemplate += '- length: ##length##\n'
          seTemplate += '- längd: ##length##\n'
        }
        if (s.height) {
          fiTemplate += '- korkeus: ##height##\n'
          enTemplate += '- height: ##height##\n'
          seTemplate += '- höjd: ##height##\n'
        }
        if (s.width) {
          fiTemplate += '- leveys: ##width##\n'
          enTemplate += '- width: ##width##\n'
          seTemplate += '- bredd: ##width##\n'
        }
        if (s.depth) {
          fiTemplate += '- syvyys: ##depth##\n'
          enTemplate += '- depth: ##depth##\n'
          seTemplate += '- djup: ##depth##\n'
        }
      }

      Object.entries(s).forEach(([key, value]) => {
        if (value.toString().length < 1) {
          fiTemplate = fiTemplate.replaceAll(`##${key}##`, 'MISSING VALUE')
          enTemplate = enTemplate.replaceAll(`##${key}##`, 'MISSING VALUE')
          seTemplate = seTemplate.replaceAll(`##${key}##`, 'MISSING VALUE')
        } else {
          fiTemplate = fiTemplate.replaceAll(`##${key}##`, value.toString())
          if (key === 'country') {
            enTemplate = enTemplate.replaceAll(`##${key}##`, countryEn[countryFi.findIndex((c) => c === value)])
            seTemplate = seTemplate.replaceAll(`##${key}##`, countrySe[countryFi.findIndex((c) => c === value)])
          } else if (key === 'riki') {
            enTemplate = enTemplate.replaceAll(`##${key}##`, riggingEn[riggingFi.findIndex((c) => c === value)])
            seTemplate = seTemplate.replaceAll(`##${key}##`, riggingSe[riggingFi.findIndex((c) => c === value)])
          } else {
            enTemplate = enTemplate.replaceAll(`##${key}##`, value.toString())
            seTemplate = seTemplate.replaceAll(`##${key}##`, value.toString())
          }
        }
      })

      newDescriptions[key] = {
        data: s,
        fi: {description: fiTemplate },
        en: {description: enTemplate },
        se: {description: seTemplate }
      }
    }
  })

  return newDescriptions
}

export default createTemplates