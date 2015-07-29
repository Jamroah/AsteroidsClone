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

public class RandomUtility
{
    /// <summary>
    /// Generate an array with its contents being
    /// unique (no numbers are repeated in the array).
    /// </summary>
    /// <param name="count">The number of items to generate.</param>
    /// <param name="min">Lower bound for the numbers generated.</param>
    /// <param name="max">Upper bound for the numbers generated.</param>
    /// <returns>An array of integers where each item in the array is unique (no numbers
    /// are repeated in the array). If count is less than zero, max is less than min or
    /// the difference between max and min is less than count, this function returns null.</returns>
    public static int[] GenerateUnique(int count, int min, int max)
    {
        if((count < 0) || (max < min) || (max - min < count)) 
        {
            return null;
        }

        int[] randomIndexes = new int[count];

        for (int i = 0; i < count; i++)
        {
            bool unique;
            int preIndex;

            do
            {
                unique = true;
                preIndex = Random.Range(min, max);
                // Check preIndex is unique
                for (int j = 0; j < i; j++)
                {
                    if (preIndex == randomIndexes[j])
                    {
                        unique = false;
                        break;
                    }
                }
            } while (!unique);

            randomIndexes[i] = preIndex;
        }

        return randomIndexes;
    }
}
