import {Audio} from '@remotion/media';
import {Series,interpolate,staticFile,useCurrentFrame,useVideoConfig} from 'remotion';
import {Opening} from './scenes/Opening';
import {Materials} from './scenes/Materials';
import {Notes} from './scenes/Notes';
import {LiveFolders} from './scenes/LiveFolders';
import {Island} from './scenes/Island';
import {Notifications} from './scenes/Notifications';
import {Finale} from './scenes/Finale';
export const FolderThemeStudioLaunch=()=>{const {fps}=useVideoConfig();const f=useCurrentFrame();return <>
<Audio name="3 Strikes — reference soundtrack" src={staticFile('reference-3-strikes.m4a')} premountFor={fps} volume={interpolate(f,[0,.25*fps,56.7*fps,57.69*fps],[0,.85,.85,0],{extrapolateLeft:'clamp',extrapolateRight:'clamp'})}/>
<Series>
<Series.Sequence name="01 — 桌面，自成一格" durationInFrames={6*fps} premountFor={fps}><Opening/></Series.Sequence>
<Series.Sequence name="02 — 12色与材质" durationInFrames={10*fps} premountFor={fps}><Materials/></Series.Sequence>
<Series.Sequence name="03 — 灵感随手记" durationInFrames={10*fps} premountFor={fps}><Notes/></Series.Sequence>
<Series.Sequence name="04 — 活文件夹" durationInFrames={8*fps} premountFor={fps}><LiveFolders/></Series.Sequence>
<Series.Sequence name="05 — 桌面灵动岛" durationInFrames={10*fps} premountFor={fps}><Island/></Series.Sequence>
<Series.Sequence name="06 — 消息与弹性拖动" durationInFrames={8*fps} premountFor={fps}><Notifications/></Series.Sequence>
<Series.Sequence name="07 — Beta18，全新出发" durationInFrames={8*fps} premountFor={fps}><Finale/></Series.Sequence>
</Series></>};
