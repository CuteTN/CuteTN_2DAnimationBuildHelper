using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameAnimationBuilder
{
    class CppCodeGenerator
    {
        Dictionary<string, AnimatingObject> data;
        Dictionary<string, int> intIds;

        public CppCodeGenerator(Dictionary<string, AnimatingObject> data)
        {
            this.data = data;
            intIds = new Dictionary<string, int>();

            int intId = 0;
            foreach(var item in data)
            {
                intIds.Add(item.Key, intId);
                intId++;
            }
        }

        #region generate input file
        private string GenerateInput_Texture()
        {
            string result = "[TEXTURES]\n";
            result += "#ID\tFilePath\n";

            foreach(var item in data)
            {
                if(item.Value is Texture)
                {
                    Texture texture = item.Value as Texture;

                    int id = intIds[texture.StringId];
                    string path = Utils.DecodeStringToRaw(texture.EncodedFilePath);

                    result += $"{id}\t{path}\n";
                }
            }

            return result;
        }

        private string GenerateInput_Sprite()
        {
            string result = "[SPRITES]\n";
            result += "#ID\tTextureId\tleft\ttop\tright\tbottom\n";

            foreach (var item in data)
            {
                if (item.Value is Sprite)
                {
                    Sprite sprite = item.Value as Sprite;

                    int id = intIds[sprite.StringId];
                    int textureId = intIds[sprite.TextureId];
                    
                    int left = sprite.Rectangle.Left;
                    int top = sprite.Rectangle.Top;
                    int right = sprite.Rectangle.Right - 1;
                    int bottom = sprite.Rectangle.Bottom - 1;

                    result += $"{id}\t{textureId}\t\t{left}\t{top}\t{right}\t{bottom}\n";
                }
            }

            return result;
        }

        private string GenerateInput_Animation()
        {
            string result = "[ANIMATIONS]\n";
            result += "#ID\tSpriteId1\tDuration1\tSpriteId2\tDuration2\t...\n";

            foreach (var item in data)
            {
                if (item.Value is Animation)
                {
                    Animation anim = item.Value as Animation;

                    int id = intIds[anim.StringId];
                    result += $"{id}\t";
                    
                    for(int i=0; i<anim.SpriteIds.Count; i++)
                    {
                        var spriteId = anim.SpriteIds[i];
                        var sprite = data[spriteId] as Sprite;
                        int spriteIntId = intIds[sprite.StringId];

                        int duration = anim.Durations[i];

                        result += $"{spriteIntId}\t\t{duration}\t\t";
                    }
                    
                    result += "\n";
                }
            }

            return result;
        }

        private string GenerateInput_StateAnimation()
        {
            string result = "[STATE_ANIMATIONS]\n";
            result += "#ID\tAnimId\tFlipX\tFlipY\tTimesR90\n";

            foreach (var item in data)
            {
                if (item.Value is StateAnimation)
                {
                    StateAnimation state = item.Value as StateAnimation;

                    int id = intIds[state.StringId];
                    int animId = intIds[state.AnimationId];
                    int flipX = state.FlipX? 1:0;
                    int flipY = state.FlipY? 1:0;
                    int timesR90 = state.TimesRotate90;

                    result += $"{id}\t{animId}\t{flipX}\t{flipY}\t{timesR90}\n";
                }
            }

            return result;
        }

        private string GenerateInput_ObjectAnimations()
        {
            string result = "[OBJECT_ANIMATIONS]\n";
            result += "#ID\tStateAnimId1\tStateAnimId2\t...\n";

            foreach (var item in data)
            {
                if (item.Value is ObjectAnimations)
                {
                    ObjectAnimations objAnims = item.Value as ObjectAnimations;

                    int id = intIds[objAnims.StringId];
                    result += $"{id}\t";

                    foreach(StateAnimation state in objAnims.States)
                    {
                        result += $"{intIds[state.StringId]}\t\t";
                    }

                    result += "\n";
                }
            }

            return result;
        }

        private string GenerateInput_CollisionBoxes()
        {
            string result = "[COLLISION_BOXES]\n";
            result += "#SpriteId\tleft\ttop\tright\tbottom\n";

            foreach (var item in data)
            {
                if (item.Value is CollisionBox)
                {
                    CollisionBox colBox = item.Value as CollisionBox;

                    int spriteId = intIds[colBox.SpriteId];

                    int left = colBox.Box.Left;
                    int top = colBox.Box.Top;
                    int right = colBox.Box.Right - 1;
                    int bottom = colBox.Box.Bottom - 1;

                    result += $"{spriteId}\t\t{left}\t{top}\t{right}\t{bottom}\n";
                }
            }

            return result;
        }

        private string GenerateInput_Sections()
        {
            string result = "[SECTIONS]\n";
            result += "#SectionId\tBackground\n";

            foreach (var item in data)
            {
                if (item.Value is Section)
                {
                    Section section = item.Value as Section;

                    int sectionId = intIds[section.StringId];
                    int bgId = intIds[section.TextureId];

                    result += $"{sectionId}\t\t{bgId}\n";
                }
            }

            return result;
        }

        private string GenerateInput_CClasses()
        {
            string result = "[CLASSES]\n";
            result += "#ClassId\tBackground\n";

            foreach (var item in data)
            {
                if (item.Value is CClass)
                {
                    CClass cClass = item.Value as CClass;

                    int cClassId = intIds[cClass.StringId];

                    result += $"{cClassId}\n";
                }
            }

            return result;
        }

        /// <summary>
        /// CuteTN Note: bad code here!
        /// </summary>
        /// <returns></returns>
        private string GenerateInput_CObjects()
        {
            string result = "[OBJECT]\n";
            result += "#ObjId\tClassId\tProperties...\n";
            
            foreach(var item in data)
            {
                if(item.Value is CObject)
                { 
                    CObject obj = item.Value as CObject;
                    int objId = intIds[obj.StringId];
                    int cclassId = intIds[obj.ClassId];
                    
                    result += $"{objId} {cclassId}\t";
                    foreach(var prop in obj.Properties)
                    {
                        string val = prop.EncodedValue;

                        // CuteTN Note: bad code here, but I don't really care because this class' whole purpose is to adapt to my game :)
                        if(prop.Type == ContextType.String)
                            val = Utils.DecodeStringToRaw(prop.EncodedValue);
                        else if(prop.Type == ContextType.Int)
                            val = prop.EncodedValue;
                        else if(prop.Type == ContextType.Bool)
                            val = Boolean.Parse(prop.EncodedValue)? "1":"0";
                        else if(intIds.ContainsKey(prop.EncodedValue))
                            val = intIds[prop.EncodedValue].ToString();

                        result += $"{prop.Name} {val}\t";
                    }

                    result += "\n";
                }
            }
            
            return result;
        }

        public string GenerateInput()
        {
            string result = "";
            result += GenerateInput_Texture() + "\n";
            result += GenerateInput_Sprite() + "\n";
            result += GenerateInput_Animation() + "\n";
            result += GenerateInput_StateAnimation() + "\n";
            result += GenerateInput_ObjectAnimations() + "\n";
            result += GenerateInput_CollisionBoxes() + "\n";
            result += GenerateInput_Sections() + "\n";
            result += GenerateInput_CClasses() + "\n";
            result += GenerateInput_CObjects() + "\n";

            return result;
        }
        #endregion

        #region generate cpp const file
        public string GenerateCppConsts()
        {
            string result = "";

            foreach(var item in intIds)
            {
                if(data[item.Key] is IAdditionalProperty)
                    continue;

                string line = $"const int {item.Key} ";

                while(line.Length < 50)
                    line += " ";
                
                line += $"= {item.Value};\n";
                result += line;
            }

            return result;
        }
        #endregion
    }
}
