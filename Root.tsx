import {Composition,Folder} from 'remotion';
import {FolderThemeStudioLaunch} from './Composition';
import {Opening} from './scenes/Opening';
import {Materials} from './scenes/Materials';
import {Notes} from './scenes/Notes';
import {LiveFolders} from './scenes/LiveFolders';
import {Island} from './scenes/Island';
import {Notifications} from './scenes/Notifications';
import {Finale} from './scenes/Finale';
export const RemotionRoot=()=><>
<Composition id="FolderThemeStudioLaunch" component={FolderThemeStudioLaunch} width={1920} height={1080} fps={30} durationInFrames={1800}/>
<Folder name="Scenes">
<Composition id="Opening" component={Opening} width={1920} height={1080} fps={30} durationInFrames={180}/>
<Composition id="Materials" component={Materials} width={1920} height={1080} fps={30} durationInFrames={300}/>
<Composition id="Notes" component={Notes} width={1920} height={1080} fps={30} durationInFrames={300}/>
<Composition id="LiveFolders" component={LiveFolders} width={1920} height={1080} fps={30} durationInFrames={240}/>
<Composition id="Island" component={Island} width={1920} height={1080} fps={30} durationInFrames={300}/>
<Composition id="Notifications" component={Notifications} width={1920} height={1080} fps={30} durationInFrames={240}/>
<Composition id="Finale" component={Finale} width={1920} height={1080} fps={30} durationInFrames={240}/>
</Folder></>;
