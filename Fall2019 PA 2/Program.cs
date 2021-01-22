using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fall2019_PA_2
{
    class Program
    {
        const double TAX_RATE = 0.09;
        const double TIP_RATE = 0.18;

        static readonly String[,] CURRENCIES = new String[7, 3] {  {"U", "1", "US Dollar"},
                                                                   {"C", ".9813", "Canadian Dollar"},
                                                                   {"E", ".757", "Euro"},
                                                                   {"I", "52.53", "Indian Rupee"},
                                                                   {"J", "80.92", "Japanese Yen"},
                                                                   {"M", "13.1544", "Mexican Peso"},
                                                                   {"B", ".6178", "British Pound"} };
        static void Main(string[] args)
        {
            int menuOption;
            int persistenceOption;
            bool persistence = true;

            while (persistence)
            {
                menuOption = 0;
                persistenceOption = 0;

                /*  Menu error-check for valid menu item selection.
                 *  Runs until user inputs a valid number (1, 2, or 3).
                 */
                while (menuOption < 1 || menuOption > 3)
                {
                    menuOption = DisplayMenu();

                    if (menuOption < 1 || menuOption > 3)
                        Console.WriteLine("\n\tError: Invalid Selection. Please select a valid option (1 - 3).\n");
                }

                //  Runs selected menu item
                switch (menuOption)
                {
                    //  Currency Converter
                    case 1:
                        Converter(ConversionMenu());
                        break;

                    //  Restaurant POS
                    case 2:
                        RestaurantPOS();
                        break;

                    //  Exit Program                
                    case 3:
                        Console.WriteLine("\nExitting program...");
                        return;
                }

                //  Return to menu or exit program 
                while(persistenceOption != 1 && persistenceOption != 2)
                {
                    Console.WriteLine("\n-----\t-----\t-----\t-----\t-----");

                    Console.WriteLine("\n\t(1)\tReturn to menu" +
                                      "\n\t(2)\tExit program" +
                                      "\n\nEnter item number to select option: ");
                    persistenceOption = int.Parse(Console.ReadLine());

                    if(persistenceOption != 1 && persistenceOption != 2)
                        Console.WriteLine("\n\tError: Invalid Selection. Please select a valid option (1 or 2).\n");
                }

                if(persistenceOption == 2)
                {
                    persistence = false;
                    Console.WriteLine("\nExitting program...");
                    return;
                }
            }
            Console.ReadLine();
        }

        //  Displays menu and returns selected menu option number
        static int DisplayMenu()
        {
            Console.WriteLine("\n-----\t-----\t MENU\t-----\t-----\n" +
                            "\n\t(1)\tConvert Currencies" +
                            "\n\t(2)\tRestaurant POS" +
                            "\n\t(3)\tExit" +
                            "\n\nEnter menu item number to select option: ");

            int option = int.Parse(Console.ReadLine());

            return option;
        }

        /*  Displays supported currencies and prompts user to
         *      select currency to convert from and to.
         *      
         *  Checks both from and to character inputs for validity and
         *      produces error if invalid and prompts user to try again.
         *  
         *  Returns a two-character string of the selected 
         *      'from' currency and 'to' currency identifiers. 
         *      (Ie. Japanese Yen to Euro would return string "JE")
         */
        static String ConversionMenu()
        {
            Console.WriteLine("\n-----\t-----\t CONVERT CURRENCIES\t-----\t-----\n");
            Console.WriteLine("\nSupported Currencies:" +
                            "\n\t(U)S Dollar" + 
                            "\n\t(C)anadian Dollar" +
                            "\n\t(E)uro" +
                            "\n\t(I)ndian Rupee" +
                            "\n\t(J)apanese Yen" +
                            "\n\t(M)exican Peso" +
                            "\n\t(B)ritish Pound");

            int inputValidity = -1;
            char from = '~';
            char to = '~';

            //  Validity check of 'from' currency input
            while(inputValidity == -1)
            {
                Console.WriteLine("\nEnter currency identifier (char) to convert FROM: ");
                from = Console.ReadLine().ToUpper()[0];

                inputValidity = CurrencyArraySearcher(from);

                if (inputValidity == -1)
                    Console.WriteLine("\tError: Invalid currency identifier: (" + from + "). Please enter a valid character. ");
            }

            inputValidity = -1;

            //  Validity check of 'to' currency input
            while (inputValidity == -1)
            {
                Console.WriteLine("\nEnter currency identifier (char) to convert TO: ");
                to = Console.ReadLine().ToUpper()[0];

                inputValidity = CurrencyArraySearcher(to);

                if (inputValidity == -1)
                    Console.WriteLine("\tError: Invalid currency identifier: (" + to + "). Please enter a valid character. ");
            }

            return String.Concat(from, to);
        }

        /*  Takes two-char string input of concatenated 
         *      'from' and 'to' currency identifiers.
         *      
         *  Uses each char from input to identify respective
         *      currency conversion rates and names. 
         *      
         *  Prompts user for amount to convert and performs 
         *      the conversion and printing the result.
         */
        static void Converter(String FT)
        {
            char from = FT[0];
            char to = FT[1];
            string inputStr = "~";

            //  Establishes 'from' currency value and name 
            int fromIndex = CurrencyArraySearcher(from);
            double fromValue = double.Parse(CURRENCIES[fromIndex, 1]);
            String fromCurr = CURRENCIES[fromIndex, 2];

            //  Establishes 'to' currency value and name 
            int toIndex =  CurrencyArraySearcher(to);
            double toValue = double.Parse(CURRENCIES[toIndex, 1]);
            String toCurr = CURRENCIES[toIndex, 2];

            double amtToConvert = -1;

            while(amtToConvert < 0)
            {
                Console.WriteLine("\nEnter amount of " + fromCurr + "s to convert to " + toCurr + "s : ");
                inputStr = Console.ReadLine();

                if(StringIsNum(inputStr))
                    amtToConvert = DoubleEval(inputStr);
            }


            double convertedVal = (amtToConvert / fromValue) * toValue;

            //  Formats before and after conversion amounts for printing
            String formatAmt = String.Format("{0:#,0.##}", amtToConvert);
            String formatResult = String.Format("{0:#,0.00}", convertedVal);

            Console.WriteLine("\n" + formatAmt + " " + fromCurr + "(s) converts to " + formatResult + " " + toCurr + "(s)\t\t(Rounded to 2 decimal places)");

            return;
        }

        /*  Searches the CURRENCY matrix for a matching 
         *      currency ID and returns the index when found
         *      
         *  Returns -1 if the currency ID is not found
        */
        static int CurrencyArraySearcher(char currID)
        {
            for(int i = 0; i < 7; i++)
            {
                if (CURRENCIES[i, 0][0] == currID)
                    return i;
            }
            return -1;
        }

        /*  Prompts user to enter food and alcohol totals.
         *  
         *  Calls Recepit() method to calculate and print recepit.
         *  
         *  Prompts user to enter amount paid and calculates amount
         *      owed based on difference between total bill and paid.
         *      
         *      If amount owed > 0, produces an error and continues 
         *      to prompts user to enter amount paid until its >= 0.
         *      
         *      If amount paid > amount owed, outputs amount of change due. 
         */
        static void RestaurantPOS()
        {
            string inputStr = "~";
            double foodTotal = -1;
            double alcTotal = -1;
            Console.WriteLine("\n-----\t-----\t RESTAURANT POS\t-----\t-----\n");

            while(foodTotal == -1)
            {
                Console.WriteLine("\nEnter food total: ");
                inputStr = Console.ReadLine();

                if(StringIsNum(inputStr))
                    foodTotal = DoubleEval(inputStr);
            }

            while(alcTotal == -1)
            {
                Console.WriteLine("\nEnter alcohol total: ");
                inputStr = Console.ReadLine();

                if(StringIsNum(inputStr))
                    alcTotal = DoubleEval(inputStr);
            }


            double totalBill = Math.Round(Receipt(foodTotal, alcTotal), 2);
            double amtPaid = 0;
            double amtOwed = totalBill;

            //  Amount paid and amount owed loop; exits loop when amount owed <= 0
            while(amtOwed > 0)
            {
                Console.WriteLine("\nPlease enter the amount paid: ");
                inputStr = Console.ReadLine();

                if(StringIsNum(inputStr))
                {
                    amtPaid = double.Parse(inputStr);

                    amtOwed -= amtPaid;

                    amtOwed = Math.Round(amtOwed, 2);

                    if (amtOwed > 0)
                    {
                        Console.WriteLine("\n\tError:\tAmount paid is less than amount owed.");
                        Console.WriteLine(String.Format("\n{0:New amount owed = #,0.00}", amtOwed));
                    }
                }
            }
            amtOwed = (Math.Round(amtOwed, 2) * -1);

            if(amtOwed != 0)
            {
                Console.WriteLine(String.Format("\n{0:Change due = #,0.00}", amtOwed));
            }

        }

        /*  Takes food total and alcohol total as inputs and calcualates
         *      tip amount, subtotal, tax amount, and total bill using
         *      tip rate and tax rate constants. 
         *      
         *  Prompts user if they would like a full receipt printed.
         *  
         *      Full receipt includes food and alcohol total, tip,
         *      subtotal, tax, and total bill.
         *      
         *      If user does not want full receipt, only prints total bill.
         * 
         *  Returns total bill double.
         */
        static double Receipt(double foodTotal, double alcTotal)
        {
            double tipAmt = (foodTotal * TIP_RATE);
            double subtotal = (foodTotal + alcTotal + tipAmt);
            double taxAmt = subtotal * TAX_RATE;
            double totalBill = (subtotal + taxAmt);

            double temp = 0;
            double cents = 0;
            double prevCent = 0;

            int x = 40;

            char choice = '~';

            //  Print full receipt prompt and input reader
            while(choice != 'Y' && choice != 'N')
            {
                Console.WriteLine("\nPrint full recepit?\t(Y) / (N)");
                choice = Console.ReadLine().ToUpper()[0];

                if(choice != 'Y' && choice != 'N')
                    Console.WriteLine("\n\tError:\tInvalid input. Please enter either (Y) or (N):");
            }

            //  Full recepit option
            if(choice == 'Y')
            {
                temp = Math.Floor(foodTotal);
                cents += (foodTotal - temp);
                prevCent = cents;
                if(foodTotal.ToString().Length >= 14)
                {
                    Console.WriteLine("\nFood" + String.Format("{0," + (x - 7) + ":$ #,0}", foodTotal) + String.Format("{0:#.00}", cents));
                }
                else
                {
                    Console.WriteLine("\nFood" + String.Format("{0," + (x - 4) + ":$ #,0.00}", foodTotal));
                }

                temp = Math.Floor(alcTotal);
                cents += (alcTotal - temp);
                if (alcTotal.ToString().Length >= 14)
                {
                    Console.WriteLine("Alcohol" + String.Format("{0," + (x - 10) + ":$ #,0}", alcTotal) + String.Format("{0:#.00}", (cents - prevCent)));
                }
                else
                {
                    Console.WriteLine("Alcohol" + String.Format("{0," + (x - 7) + ":$ #,0.00}", alcTotal));
                }

                temp = Math.Floor(tipAmt);
                cents += (tipAmt - temp);
                Console.WriteLine("Tip" + String.Format("{0," + (x - 3) + ":$ #,0.00}", tipAmt));

                if (subtotal.ToString().Length >= 15)
                {
                    Console.WriteLine("Subtotal" + String.Format("{0," + (x - 11) + ":$ #,0}", subtotal) + String.Format("{0:#.00}", cents));
                }
                else
                {
                    Console.WriteLine("Subtotal" + String.Format("{0," + (x - 8) + ":$ #,0.00}", subtotal));
                }

                temp = Math.Floor(taxAmt);
                cents += (taxAmt - temp);
                Console.WriteLine("Tax" + String.Format("{0," + (x - 3) + ":$ #,0.00}", taxAmt));

            }


            if (totalBill.ToString().Length >= 15)
            {
                temp = Math.Floor(totalBill);
                cents = (totalBill - temp);
                Console.WriteLine("\nTotal due" + String.Format("{0," + (x - 12) + ":$ #,0}", totalBill) + String.Format("{0:#.00}", cents));
            }
            else
            {
                Console.WriteLine("\nTotal due" + String.Format("{0," + (x - 9) + ":$ #,0.00}", totalBill));
            }

            return totalBill;
        }

        /*  Takes a string input to determine whether or not it is a valid number:
         * 
         *      Produces error and returns false if string has a negative sign, contains
         *          a non-numeric character, has no digits, or has more than 1 decimal.
         *      
         *      Return true if string has at least 1 digit and at most 1 decimal.
         */
        static bool StringIsNum(string input)
        {
            int numDigits = 0;
            int numDecimals = 0;

            //  Produces error and returns false because a negative sign was found in the first index
            if(input[0] == '-')
            {
                Console.WriteLine("\n\tError:\tNegative sign detected. Please enter a positive number to convert.\n");
                return false;
            }

            /*  Loops through each char in input and checks individual characters
             *      Counts number of digits found and number of decimals found
             */
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsDigit(input[i]))
                    numDigits++;

                if (input[i] == '.')
                    numDecimals++;

                //  Produces an error and returns false because a non-numerical character was found in the string
                if (!char.IsDigit(input[i]) && input[i] != '.')
                {
                    Console.WriteLine("\n\tError:\tNon-numerical character detected (" + input[i] + "). Please enter a valid number to convert.\n");
                    return false;
                }

            }

            //  Produces error and returns false because no numerical characters were found and more than 1 decimal was found in the string
            if (numDigits == 0 && numDecimals > 1)
            {
                Console.WriteLine("\n\tError:\tNumber of digits = 0 and number of decimals > 1. Please enter a valid number to convert.\n");
                return false;
            }

            //  Produces error and returns false because no numerical characters were found in the string
            if (numDigits == 0)
            {
                Console.WriteLine("\n\tError:\tNumber of digits = 0. Please enter a valid number to convert.\n");
                return false;
            }

            //  Produces error and returns false because more than 1 decimal was found in the string
            if (numDecimals > 1)
            {
                Console.WriteLine("\n\tError:\tNumber of decimals > 1. Please enter a valid number to convert.\n");
                return false;
            }

            return true;
        }

        /*  Evaluates a string input before returning input string parsed as a double.
         *  
         *  If input contains 14 or 15 digits, warns user that the product of calculations
         *      using that number may be imprecise or rounded automatically.
         *  
         *  If input contains more than 15 digits, warns user that it exceeds double limit but 
         *      calculation can still be done as a rounded 15 digits + concatenated 0's.
         */
        static double DoubleEval(string input)
        {
            int numDigits = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsDigit(input[i]))
                    numDigits++;
            }

            if(numDigits == 14 || numDigits == 15)
            {
                Console.WriteLine("\n\tWarning:\tInput is a very large number (" + numDigits + " digits) and approaches double limit." +
                                  "\n\t        \tResults from calculations involving this number may be imprecise or rounded automatically.");

            }

            if (numDigits > 15)
            {
                Console.WriteLine("\n\tWarning:\tNumber of digits is greater than 15 and exceeds double limit." + 
                                  "\n\t        \tCalculation will be done but input will be rounded to 15 precise digits concatenated with " + (numDigits - 15) + " empty digits.");
            }

            return double.Parse(input);
        }
    }
}
