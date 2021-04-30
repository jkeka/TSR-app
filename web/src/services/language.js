class LangSrv {
  constructor() {
    this.languages = [ 'fi', 'en', 'se' ]
    this.getLanguages = this.getLanguages.bind(this)
  }
  getLanguages()  {
    return this.languages
  }
}

const langSrv = new LangSrv()

export default langSrv