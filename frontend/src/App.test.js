import {queryByAttribute, render, screen} from '@testing-library/react';
import React from 'react';
import App from './App';

describe("App Unit Tests", () => {

  test('app renders turn on/off text', () => {
    render(<App />);
    const textElement = screen.getByText("Turn ON/OFF");
    expect(textElement).toBeInTheDocument();
  });

  test('Testing Sunrise and Sunset Switch', async () => {
    const dom = render(<App />);
    const getById = queryByAttribute.bind(null, 'id');

    const muiSwitch = getById(dom.container, 'srss');
    expect(muiSwitch.checked).toBe(false);

    await muiSwitch.click();
    expect(muiSwitch).toBeChecked();
  });

  test('App renders victory chart', () => {
    const dom = render(<App />);
    const getByClass = queryByAttribute.bind(null, 'class');

    const vcElement = getByClass(dom.container, 'VictoryContainer');
    expect(vcElement).toBeInTheDocument();
  });

});