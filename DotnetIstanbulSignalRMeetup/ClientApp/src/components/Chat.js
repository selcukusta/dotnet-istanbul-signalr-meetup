import React, { Component } from 'react';
import * as signalR from '@aspnet/signalr';

export class Chat extends Component {
    constructor(props) {
        super(props);

        this.state = {
            message: '',
            messages: [],
            hubConnection: null,
        };
    }

    componentDidMount = () => {
        const hubConnection = new signalR.HubConnectionBuilder()
            .withUrl("hubs/chat")
            .build();       

        this.setState({ hubConnection }, () => {           
            this.state.hubConnection
                .start()
                .then(() => console.log('Connection started!'))
                .catch(err => console.log('Error while establishing connection :('));

            this.state.hubConnection.on('SendMessage', (receivedMessage) => {
                const messages = this.state.messages.concat([receivedMessage]);
                this.setState({ messages });
            });
        })
    }

    sendMessage = () => {
        this.state.hubConnection
            .invoke('Send', this.state.message)
            .catch(err => console.error(err));

        this.setState({ message: '' });
    };

    render() {
        return (
            <div>
                <input
                    type="text"
                    value={this.state.message}
                    onChange={e => this.setState({ message: e.target.value })}
                />

                <button onClick={this.sendMessage}>Send</button>

                <div>
                    {this.state.messages.map((message, index) => (
                        <span style={{ display: 'block' }} key={index}> {message} </span>
                    ))}
                </div>
            </div>
        );
    }
}