import React from 'react';
import config from '../config.json';


class WordCounterForm extends React.Component {
    constructor(props){
        super(props);
        this.state = {text:'', words: []};
        this.handleChange = this.handleChange.bind(this);
        this.getWordCount = this.getWordCount.bind(this);
    }
    handleChange(event) {
        this.setState({text: event.target.value});
      }

    render(){
        return <div>
                    <div className="row">
                        <div className="col">
                            <textarea className="form-control" onChange={this.handleChange} value={this.state.text}></textarea>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col pt-2">
                            <button className="btn btn-primary" onClick={this.getWordCount}>Count</button>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col pt-2">
                            <ul>
                                {this.state.words.map(function(word, index){
                                    return <li key={ index }>{word.word} ({word.count})</li>;
                                })}
                            </ul>
                        </div>
                    </div>
                 </div>
    }
    getWordCount(){
        fetch(config.WORD_COUNTER_URL+"api/v1/word-count/"+this.state.text)
        .then(res => res.json())
        .then( (result) => {
            this.setState({
              words: result
            });
        }).catch(error=>{
            console.log(error);
        });
    }
}

export default WordCounterForm;