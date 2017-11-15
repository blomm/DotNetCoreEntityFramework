import React, { Component } from 'react';
import logo from './logo.svg';
import './App.css';
import {Orders} from './Orders';
import {Products} from './Products';

class App extends Component {

    //data = {};
    
    constructor(){
      super();
      this.state = {
        products: []
      };
      //let data = {};
    }

    componentDidMount(){
        fetch('http://localhost:5000/api/dutch/products')
        .then((resp) => 
            resp.json()
        ) // Transform the data into json
        .then((products) =>
            
            this.setState({ products })
        )
    }

    render(){
        return (
            <div>
                <Orders/>
                <Products dutchProducts={this.state.products}/>
            </div>
        )
    }
    
}

export default App;
