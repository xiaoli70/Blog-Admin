/**
 * 随机整数
 * @param min 最小值
 * @param max 最大值
 * @returns
 */
export const randomNumber = (min: number, max: number) => {
  return Math.floor(Math.random() * (max - min + 1)) + min;
};
