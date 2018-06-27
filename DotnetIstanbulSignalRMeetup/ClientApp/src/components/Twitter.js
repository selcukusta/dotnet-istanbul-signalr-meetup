import React, { Component } from 'react';
import * as signalR from '@aspnet/signalr';

export class Twitter extends Component {
    constructor(props) {
        super(props);
        this.state = {
            tweets: [],
            hubConnection: null,
        };
    }

    componentDidMount = () => {
        const hubConnection = new signalR.HubConnectionBuilder()
            .withUrl("hubs/twitter")
            .build();

        this.setState({ hubConnection }, () => {
            this.state.hubConnection
                .start()
                .then(() => {
                    console.warn('Connection started!');
                    this.state.hubConnection
                        .invoke('Start')
                        .catch(err => console.error(err));
                }).catch(err => console.error('Error while establishing connection :('));

            this.state.hubConnection.on('SendTweet', (receivedMessage) => {
                const tweets = this.state.tweets.concat([receivedMessage]);
                this.setState({ tweets });
            });
        })
    }

    render() {
        return (
            <div>
                <div>
                    <ul>
                        {this.state.tweets.map((message, index) => (
                            <li key={index}> {message} </li>
                        ))}
                    </ul>
                </div>
            </div>
        );
    }
}

