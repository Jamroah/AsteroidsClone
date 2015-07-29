/*
 * Copyright (c) 2014 
 *      Luke Hale
 *      Niall Frederick Weedon
 *      Timothy Stanley
 *      Jon Langford                
 *      Charlotte Croucher
 *      Daniel Eeles
 *      Armani Gentles-Williams
 *      David Johnson
 *      Ben Beagley
 *      Ben Roberts
 *      William Hurst
 *      Matthew Larwood
 *      Mat Greenhalgh
 *      Greg Martin-Pavitt
 *      William Ch'ng Guo Wei
 *      Joe Larkin
 *      
 * Permission is hereby granted to Mark Eyles to use (the "Software"), to deal
 * in the Software with restriction, including only the rights to use, distribute
 * and/or view the source of the Software, and to permit persons to whom the 
 * Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included
 * in all copies or substantial portions of the Software, usage and distribution
 * will be subject to a request of the individuals listed above, and royalties 
 * will be paid to the named individuals listed above, after usage and/or distribution.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE 
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE 
 * USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using UnityEngine;
using System.Collections;

public class GameVersionUtility : MonoBehaviour
{
    public string versionString;
    public bool isLiteVersion;

    private static GameVersionUtility s_instance;

    /// <summary>
    /// Determines whether the game is the lite version.
    /// To set the game to the lite version, include the
    /// 'INCLUDE-IN-LITE-BUILD' scene when building the game
    /// on any platform.
    /// </summary>
    /// <returns></returns>
    public bool IsLiteVersion() 
    {
        return Application.CanStreamedLevelBeLoaded("INCLUDE-IN-LITE-BUILD") || isLiteVersion;
    }

    /// <summary>
    /// Gets the string of the current version
    /// </summary>
    /// <returns></returns>
    public string GetVersionString()
    {
        if (IsLiteVersion())
        {
            return string.Format("Lite\n{0}", versionString);
        }
        else
        {
            return versionString;
        }
    }
}