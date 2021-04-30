import React, { Component } from 'react'
import firebase from '../services/firebase'



export default class Home extends Component {

    constructor(props) {
        super(props)
        this.title = 'Home'
        this.storageRef = firebase.storage().ref();
        this.state = { testi: '', selectedFile: null, image: null, startTime: new Date() }
        this.submitData = this.submitData.bind(this)
        this.handleChange = this.handleChange.bind(this)
        this.getData = this.getData.bind(this)
    }
    componentDidMount() {
        const ref = firebase.database().ref()
        
        ref.on('value', (snapshot) => {
            console.log(snapshot.val())
            this.setState({testi: JSON.stringify(snapshot.val(), null, 2)})
        })
        
    }
    submitData(e) {
        e.preventDefault()
        console.log('click!')

        if(this.state.selectedFile !== null) {
            this.storageRef.child('testi').put(this.state.selectedFile)
                .then((snapshot) => {
                    console.log('uploaded something')
                    console.log(snapshot)
                })
        }
    }
    handleChange(e) {
        console.log('changed!')
        console.log(e.target.files[0])
        this.setState({selectedFile: e.target.files[0]})
    }
    getData() {
        let imgRef = firebase.storage().ref('testi')
        imgRef.getDownloadURL()
            .then((url) => this.setState({image: url}))
            .catch(e => console.log(e))
        
    }
    render() {
        return (
            
            <div>
                <h1>
                   {this.title} 
                </h1>
                {/**
                <form method="post" enctype="multipart/form-data" onSubmit={this.submitData}>
                <label for="imageUpload">Choose a profile picture:</label>
                <br/>
                <input type="file"
                    id="imageUpload" name="imageUpload"
                    accept="image/png, image/jpeg" onChange={this.handleChange} />
                <br/>
                <button type="submit">Submit</button>
                </form>
                <button onClick={() => this.getData()}>Get data</button>
                <img alt="" src={this.state.image} /> */}
            </div>
        )
    }
}