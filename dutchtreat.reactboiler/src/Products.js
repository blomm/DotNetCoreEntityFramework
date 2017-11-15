import React, { Component } from 'react';

export class Products extends Component{
    
    // constructor(){
    //     super();
    // }

    render(){
        return(
            <div>
                These are the products...
                <ul>
                    {this.props.dutchProducts.map(item =>
                        <li key={item.id}>{item.category} : {item.title}</li>
                    )}
                </ul>
            </div>
        )
    }
}    