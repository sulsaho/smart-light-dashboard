//import logo from './logo.svg';
import './App.css';
import Button from '@material-ui/core/Button';
import ButtonGroup from '@material-ui/core/ButtonGroup'
import Slider from '@material-ui/core/Slider';
import BrightnessLowIcon from '@mui/icons-material/BrightnessLow';
import BrightnessHighIcon from '@mui/icons-material/BrightnessHigh';
import axios from 'axios';
import Checkbox from '@material-ui/core/Checkbox';
import {Stack} from "@mui/material";
import {FormControl, FormControlLabel, Radio, RadioGroup, Switch, TextField} from "@material-ui/core";
import {useState} from "react";
import MaterialUISwitch from './MUISwitch';

function App() {
  const [time, setTime] = useState('');
  const [onOff, setChecked] = useState('');

  function handleTime (event) {
    setTime(event.target.value);
  }
  function setSchedule (event){
    setChecked(event.target.value);
    console.log(onOff);
  }
  async function postSchedule(){
    await axios.post(`http://localhost:5000/api/LightState/light/get-schedule/${onOff}`);
  }

  async function postTime(){
    await axios.post(`https://localhost:5001/api/LightState/light/get-time/${time}`);
  }

  async function onButton() {
    await axios.post('http://localhost:5000/api/LightState/light/turn-on');
  }

  async function offButton() {
    await axios.post('https://localhost:5001/api/LightState/light/turn-off');
  }

  async function setWhiteColor() {
    await axios.post('https://localhost:5001/api/LightState/light/color/white');
  }

  async function setRedColor() {
    await axios.post('https://localhost:5001/api/LightState/light/color/red');
  }

  async function setOrangeColor() {
    await axios.post('https://localhost:5001/api/LightState/light/color/orange');
  }

  async function setYellowColor() {
    await axios.post('https://localhost:5001/api/LightState/light/color/yellow');
  }

  async function setCyanColor() {
    await axios.post('https://localhost:5001/api/LightState/light/color/cyan');
  }

  async function setGreenColor() {
    await axios.post('https://localhost:5001/api/LightState/light/color/green');
  }

  async function setBlueColor() {
    await axios.post('https://localhost:5001/api/LightState/light/color/blue');
  }

  async function setPurpleColor() {
    await axios.post('https://localhost:5001/api/LightState/light/color/purple');
  }

  async function setPinkColor() {
    await axios.post('https://localhost:5001/api/LightState/light/color/pink');
  }

  async function breatheEffect() {
    await axios.post('https://localhost:5001/api/LightState/light/breathe');
  }

  async function getBrightnessValue(e, val) {
    let uri = 'https://localhost:5001/api/LightState/light/brightness/' + val.toString();
    await axios.post(uri);
  }

  return (
    <div className="App">
      <header className="App-header">
        <div>
          <h2>Turn ON/OFF</h2>
          <ButtonGroup
            variant="outlined">
            <Button
                size="large"
                color="primary"
                onClick={onButton}>
              ON
            </Button>
            <Button
                size="large"
                color="secondary"
                onClick={offButton}>
              OFF
            </Button>
          </ButtonGroup>
        </div>

        <div>
          <h2>Set Colors</h2>
          <ButtonGroup
            variant='outlined'>
            <Button
                size="large"
                style={{color : '#FFFFFF'}}
                onClick={setWhiteColor}>
              White
            </Button>
            <Button
                size="large"
                style={{color : '#FF0000'}}
                onClick={setRedColor}>
              Red
            </Button>
            <Button
                size="large"
                style={{color : '#FFA500'}}
                onClick={setOrangeColor}>
              Orange
            </Button>
            <Button
                size="large"
                style={{color : '#FFFF00'}}
                onClick={setYellowColor}>
              Yellow
            </Button>
            <Button
                size="large"
                style={{color : '#00FFFF'}}
                onClick={setCyanColor}>
              Cyan
            </Button>
            <Button
                size="large"
                style={{color : '#008000'}}
                onClick={setGreenColor}>
              Green
            </Button>
            <Button
                size="large"
                style={{color : '#0000FF'}}
                onClick={setBlueColor}>
              Blue
            </Button>
            <Button
                size="large"
                style={{color : '#800080'}}
                onClick={setPurpleColor}>
              Purple
            </Button>
            <Button
                size="large"
                style={{color : '#FF00FF'}}
                onClick={setPinkColor}>
              Pink
            </Button>
          </ButtonGroup>
        </div>

        <h2>Pulse Effect</h2>
        <div>
          <Button
              variant="contained"
              size="large"
              color="primary"
              onClick={breatheEffect}>
            Pulse Light
          </Button>
        </div>

        <h2>Set Brightness</h2>

        <div style={{width: 300}}>
          <Stack spacing={2} direction="row" sx={{ mb: 1 }} alignItems="center">
            <BrightnessLowIcon />
            <Slider
              color="primary"
              defaultValue={50}
              step={10}
              marks
              valueLabelDisplay={"auto"}
              onChange={getBrightnessValue}>
            </Slider>
            <BrightnessHighIcon />
          </Stack>
        </div>

        {/*<Checkbox/> */}

          {/* Not needed */}
        {/*<img src={logo} className="App-logo" alt="logo" /> */}
        <div>
          <h2>Set the schedule</h2>
          <Stack spacing={2} direction="row" sx={{ mb: 1 }} alignItems="center">
            {/*<Switch
                checked={checked}
                onChange={handleSchedule}
                inputProps={{ 'aria-label': 'controlled' }}
            />*/}
            <FormControl component="fieldset">
              <RadioGroup
                  aria-label="onOff"
                  name="controlled-radio-buttons-group"
                  value={onOff}
                  onChange={setSchedule}>
                <FormControlLabel value="ON" control={<Radio />} label="ON" />
                <FormControlLabel value="OFF" control={<Radio />} label="OFF" />
              </RadioGroup>
            </FormControl>
            <TextField id="outlined-basic"  variant="filled" type="time" onChange={handleTime}/>
            <Button variant="contained" onClick={postTime}>Save</Button>
            <Button variant="contained" onClick={postSchedule}>Submit</Button>
          </Stack>
        </div>
      </header>
    </div>

  );
}

export default App;
