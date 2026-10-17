import {interpolate,useCurrentFrame,useVideoConfig} from 'remotion';
import {DemoLabel,Eyebrow,Headline,Stage,Sub,ease,pop} from '../design';
export const Island=()=>{const f=useCurrentFrame();const {fps}=useVideoConfig();return <Stage dark>
<div style={{position:'absolute',left:115,top:105}}><Eyebrow dark>DESKTOP ISLAND</Eyebrow><Headline style={{marginTop:35}}>小小一岛。<br/>刚刚好。</Headline></div>
<div style={{position:'absolute',right:130,top:170,fontSize:130,fontWeight:700,color:'#33333a',letterSpacing:-9}}>260 × 64</div>
<div style={{position:'absolute',left:380,top:565,width:1160,height:1160*52/248,borderRadius:1160*26/248,border:'2px solid #36363b',background:'linear-gradient(145deg,#202023,#080809)',boxShadow:'0 40px 100px #000',scale:pop(f,fps,10),translate:`0 ${interpolate(f,[100,135],[0,-25],ease)}px`,overflow:'hidden',display:'flex',flexDirection:'column',alignItems:'center',justifyContent:'center',gap:22}}>
<div style={{display:'flex',alignItems:'center',gap:30,fontFamily:'Consolas, monospace',fontSize:40,letterSpacing:1,color:'#e8e8ec'}}><span style={{width:13,height:13,borderRadius:'50%',background:'#6bdaa1'}}/><span>19:54</span><span style={{color:'#9be8be'}}>CPU 24%</span><span style={{color:'#9be8be'}}>RAM 48%</span></div>
<div style={{fontSize:37,color:'#b5b5bd',letterSpacing:2,opacity:interpolate(f,[80,110],[0,1],ease)}}>{f<200?'让每一份灵感，都找到自己的位置':'把喜欢的日常，轻轻放在眼前'}</div>
</div>
<Sub style={{position:'absolute',left:0,right:0,bottom:125,textAlign:'center',fontSize:42,color:'#96969e'}}>同步歌词 · 右键控制音乐</Sub><DemoLabel dark/>
</Stage>};
